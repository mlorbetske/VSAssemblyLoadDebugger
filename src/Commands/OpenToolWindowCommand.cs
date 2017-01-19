using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;

namespace AssemblyLoadDebugger
{
    internal sealed class OpenToolWindowCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("852872c5-92a2-4343-9bdd-ffd69e03f226");

        private readonly Package package;

        private OpenToolWindowCommand(Package package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            var cmdId = new CommandID(CommandSet, CommandId);
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
