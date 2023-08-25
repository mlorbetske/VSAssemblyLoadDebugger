using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using AssemblyLoadDebugger;
using Commands;
using Microsoft.VisualStudio.Shell;
using ToolWindow;

[PackageRegistration(AllowsBackgroundLoading = true, UseManagedResourcesOnly = true)]
[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[ProvideToolWindow(typeof(AssemblyLoadDebuggerToolWindow))]
[Guid(PackageGuids.guidAssemblyLoadDebuggerPackageString)]
public sealed class VSPackage : AsyncPackage
{
    protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
        {
            OpenToolWindowCommand.Initialize(this, commandService);
        }
    }
}