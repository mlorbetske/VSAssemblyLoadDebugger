using System;
using System.ComponentModel.Design;
using AssemblyLoadDebugger;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ToolWindow;

namespace Commands
{
    internal sealed class OpenToolWindowCommand
    {
        private readonly Package package;

        private OpenToolWindowCommand(Package package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            var cmdId = new CommandID(PackageGuids.guidAssemblyLoadDebuggerPackageCmdSet, PackageIds.AssemblyLoadDebuggerCommandId);
            var cmd = new MenuCommand(ShowToolWindow, cmdId);
            commandService.AddCommand(cmd);
        }

        public static OpenToolWindowCommand Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get { return package; }
        }

        public static void Initialize(Package package, OleMenuCommandService commandService)
        {
            Instance = new OpenToolWindowCommand(package, commandService);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            ToolWindowPane window = package.FindToolWindow(typeof(AssemblyLoadDebuggerToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
