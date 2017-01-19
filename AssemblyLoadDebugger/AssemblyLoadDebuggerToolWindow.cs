//------------------------------------------------------------------------------
// <copyright file="AssemblyLoadDebugger.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AssemblyLoadDebugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("71604717-b15d-4d1e-9fb4-03f5122e8858")]
    public class AssemblyLoadDebuggerToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoadDebugger"/> class.
        /// </summary>
        public AssemblyLoadDebuggerToolWindow() : base(null)
        {
            this.Caption = "AssemblyLoadDebugger";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            AssemblyLoadDebuggerControl control = new AssemblyLoadDebuggerControl();
            control.DataContext = AssemblyLoadDebuggerControlViewModel.Instance;
            this.Content = control;
        }
    }
}
