﻿using ClangPowerTools.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace ClangPowerTools.Commands
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal sealed class StopCommand : ClangCommand
  {
    #region Members

    private PCHCleaner mPCHCleaner = new PCHCleaner();

    #endregion


    #region Properties


    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static StopCommand Instance
    {
      get;
      private set;
    }


    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StopCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    private StopCommand(OleMenuCommandService aCommandService, CommandsController aCommandsController,
      AsyncPackage aPackage, Guid aGuid, int aId)
      : base(aPackage, aGuid, aId)
    {
      if (null != aCommandService)
      {
        var menuCommandID = new CommandID(CommandSet, Id);
        var menuCommand = new OleMenuCommand(aCommandsController.Execute, menuCommandID);
        menuCommand.BeforeQueryStatus += aCommandsController.OnBeforeClangCommand;
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
    public static async Task InitializeAsync(CommandsController aCommandsController,
      AsyncPackage aPackage, Guid aGuid, int aId)
    {
      // Switch to the main thread - the call to AddCommand in StopClang's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(aPackage.DisposalToken);

      OleMenuCommandService commandService = await aPackage.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
      Instance = new StopCommand(commandService, aCommandsController, aPackage, aGuid, aId);
    }


    public Task RunStopClangCommandAsync()
    {
      StopCommand = true;
      return System.Threading.Tasks.Task.Run(() =>
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

