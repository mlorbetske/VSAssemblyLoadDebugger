namespace AssemblyLoadDebugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    [Guid("71604717-b15d-4d1e-9fb4-03f5122e8858")]
    public class AssemblyLoadDebuggerToolWindow : ToolWindowPane
    {
        public AssemblyLoadDebuggerToolWindow() : base(null)
        {
            this.Caption = Vsix.Name;

            AssemblyLoadDebuggerControl control = new AssemblyLoadDebuggerControl();
            control.DataContext = AssemblyLoadDebuggerControlViewModel.Instance;
            this.Content = control;
        }
    }
}
