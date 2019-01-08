﻿using ClangPowerTools.Output;
using ClangPowerTools.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;

namespace ClangPowerTools.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class IgnoreCommand : BasicCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        #region Properties

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static IgnoreCommand Instance
        {
            get;
            private set;
        }
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private IgnoreCommand(CommandsController aCommandsController, OleMenuCommandService aCommandService, AsyncPackage aPackage, Guid aGuid, int aId)
          : base(aPackage, aGuid, aId)
        {
            if (null != aCommandService)
            {
                var menuCommandID = new CommandID(CommandSet, Id);
                var menuItem = new OleMenuCommand(aCommandsController.Execute, menuCommandID);
                aCommandService.AddCommand(menuItem);
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async System.Threading.Tasks.Task InitializeAsync(CommandsController aCommandsController,
          ErrorWindowController aErrorWindow, OutputWindowController aOutputWindow, AsyncPackage aPackage, Guid aGuid, int aId)
        {
            // Switch to the main thread - the call to AddCommand in StopClang's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(aPackage.DisposalToken);

            OleMenuCommandService commandService = await aPackage.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new IgnoreCommand(aCommandsController, commandService, aPackage, aGuid, aId);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void RunIgnoreCommand()
        {

        }
        #endregion
    }
}
