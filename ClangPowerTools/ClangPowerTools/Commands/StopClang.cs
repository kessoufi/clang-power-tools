﻿using ClangPowerTools.Output;
using ClangPowerTools.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;

namespace ClangPowerTools.Commands
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal sealed class StopClang : ClangCommand
  {
    #region Members

    private PCHCleaner mPCHCleaner = new PCHCleaner();

    #endregion


    #region Properties


    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static StopClang Instance
    {
      get;
      private set;
    }


    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StopClang"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    private StopClang(OleMenuCommandService aCommandService, CommandsController aCommandsController, ErrorWindowController aErrorWindow, OutputWindowController aOutputWindow,
      IVsSolution aSolution, AsyncPackage aPackage, Guid aGuid, int aId)
      : base(aCommandsController, aErrorWindow, aOutputWindow, aSolution, aPackage, aGuid, aId)
    {
      if (null != aCommandService)
      {
        var menuCommandID = new CommandID(CommandSet, Id);
        var menuCommand = new OleMenuCommand(this.RunStopClangCommand, menuCommandID);
        menuCommand.BeforeQueryStatus += mCommandsController.OnBeforeClangCommand;
        menuCommand.Enabled = true;
        aCommandService.AddCommand(menuCommand);
      }
    }

    #endregion


    #region Public Methods


    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async System.Threading.Tasks.Task InitializeAsync(CommandsController aCommandsController, ErrorWindowController aErrorWindow,
      OutputWindowController aOutputWindow, IVsSolution aSolution, AsyncPackage aPackage, Guid aGuid, int aId)
    {
      // Switch to the main thread - the call to AddCommand in StopClang's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(aPackage.DisposalToken);

      OleMenuCommandService commandService = await aPackage.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
      Instance = new StopClang(commandService, aCommandsController, aErrorWindow, aOutputWindow, aSolution, aPackage, aGuid, aId);
    }


    #endregion


    #region Private Methods 

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void RunStopClangCommand(object sender, EventArgs e)
    {
      mCommandsController.Running = false;

      System.Threading.Tasks.Task.Run(() =>
      {
        try
        {
          mRunningProcesses.Kill();
          if (VsServiceProvider.TryGetService(typeof(DTE), out object dte))
          {
            string solutionPath = (dte as DTE2).Solution.FullName;
            string solutionFolder = solutionPath.Substring(0, solutionPath.LastIndexOf('\\'));
            mPCHCleaner.Remove(solutionFolder);
          }
          mDirectoriesPath.Clear();
        }
        catch (Exception) { }
      });
    }

    #endregion


  }
}

