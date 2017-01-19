using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;

namespace AssemblyLoadDebugger
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(AssemblyLoadDebuggerToolWindow))]
    [Guid(PackageGuidString)]
    public sealed class AssemblyLoadDebuggerPackage : AsyncPackage
    {
        public const string PackageGuidString = "b2195adc-e364-4d6e-9d08-74b500b223f4";

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                OpenToolWindowCommand.Initialize(this, commandService);
            }
        }
    }
}
