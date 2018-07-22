﻿using ClangPowerTools.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using IServiceProvider = System.IServiceProvider;
using ShellConstants = Microsoft.VisualStudio.Shell.Interop.Constants;

namespace ClangPowerTools.SilentFile
{
  public class SilentFileChanger
  {
    #region Members


    private string mDocumentFileName;
    private bool mIsSuspending;
    private bool mReloadDocument;

    private AsyncPackage mSite;
    private IVsPersistDocData mPersistDocData = null;
    private IVsDocDataFileChangeControl mFileChangeControl;

    private IntPtr mDocData = new IntPtr();


    #endregion


    #region Properties

    public string DocumentFileName => mDocumentFileName;

    #endregion


    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public SilentFileChanger() { }

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aSite"></param>
    /// <param name="aDocument"></param>
    /// <param name="aReloadDocument"></param>
    public SilentFileChanger(AsyncPackage aSite, string aDocument, bool aReloadDocument)
    {
      mSite = aSite;
      mDocumentFileName = aDocument;
      mReloadDocument = aReloadDocument;
    }


    #endregion


    #region Public methods


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
    public async void Suspend()
    {
      if (mIsSuspending)
        return;

      mDocData = IntPtr.Zero;
      try
      {
        var rdtService = await mSite.GetServiceAsync(typeof(SVsRunningDocumentTableService)) as IVsRunningDocumentTableService;
        var rdt = await rdtService.GetVsRunningDocumentTableAsync(mSite, new CancellationToken());

        if (rdt == null)
          return;

        ErrorHandler.ThrowOnFailure(rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, mDocumentFileName,
          out IVsHierarchy hierarchy, out uint itemId, out mDocData, out uint docCookie));

        if ((docCookie == (uint)ShellConstants.VSDOCCOOKIE_NIL) || mDocData == IntPtr.Zero)
          return;

        var fileChangeService = await mSite.GetServiceAsync(typeof(SVsFileChangeService)) as IVsFileChangeService;
        var fileChange = await fileChangeService.GetVsFileChangeAsync(mSite, new CancellationToken());

        if (fileChange == null)
          return;

        mIsSuspending = true;
        ErrorHandler.ThrowOnFailure(fileChange.IgnoreFile(0, mDocumentFileName, 1));
        if (mDocData == IntPtr.Zero)
          return;

        mPersistDocData = null;
        object unknown = Marshal.GetObjectForIUnknown(mDocData);
        if (!(unknown is IVsPersistDocData))
          return;

        mPersistDocData = (IVsPersistDocData)unknown;
        if (!(mPersistDocData is IVsDocDataFileChangeControl))
          return;

        mFileChangeControl = (IVsDocDataFileChangeControl)mPersistDocData;
        if (mFileChangeControl != null)
          ErrorHandler.ThrowOnFailure(mFileChangeControl.IgnoreFileChanges(1));
      }
      catch (InvalidCastException e)
      {
        Trace.WriteLine("Exception" + e.Message);
      }
      finally
      {
        if (mDocData != IntPtr.Zero)
          Marshal.Release(mDocData);
      }
    }


    public async void Resume()
    {
      if (!mIsSuspending || mPersistDocData == null)
        return;

      if (mPersistDocData != null && mReloadDocument)
        mPersistDocData.ReloadDocData(0);

      var fileChangeService = await mSite.GetServiceAsync(typeof(SVsFileChangeService)) as IVsFileChangeService;
      var fileChange = await fileChangeService.GetVsFileChangeAsync(mSite, new CancellationToken());

      if (fileChange == null)
        return;

      mIsSuspending = false;
      ErrorHandler.ThrowOnFailure(fileChange.SyncFile(mDocumentFileName));
      ErrorHandler.ThrowOnFailure(fileChange.IgnoreFile(0, mDocumentFileName, 0));
      if (mFileChangeControl != null)
        ErrorHandler.ThrowOnFailure(mFileChangeControl.IgnoreFileChanges(0));
    }


    #endregion

  }
}
