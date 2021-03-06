﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AssemblyLoadDebugger
{
    public class AssemblyLoadDebuggerControlViewModel : INotifyPropertyChanged
    {
        private string _tempDirectory;
        private bool _isCapturing;
        private string _selectedBreakCondition;
        private string _userEntryBreakCondition;
        private string _searchString;
        private string _applicationInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        private Settings _settings;
        public AssemblyLoadDebuggerControlViewModel()
        {
            _applicationInfo = $"Window Title: {Process.GetCurrentProcess().MainWindowTitle} (Process ID: {Process.GetCurrentProcess().Id}){Environment.NewLine}";
            _applicationInfo += $"Main Module File: {Process.GetCurrentProcess().MainModule.FileName}{Environment.NewLine}";
            _applicationInfo += $"Main Module Version: {Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion}{Environment.NewLine}";

            Events = new ObservableCollection<string>();
            FilteredEvents = CollectionViewSource.GetDefaultView(Events);
            FilteredEvents.Filter = FilterEvents;

            string tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Directory.CreateDirectory(tempFile);
            _tempDirectory = tempFile;
            
            _settings = Settings.FromFile(SettingsFilePath);

            OpenLogCommand = ActionCommand.From<string>(file =>
            {
                DTE2 dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE2;
                dte?.ItemOperations?.OpenFile($@"{_tempDirectory}\{file.Replace(": ", "-")}.txt");
            });

            ToggleCaptureCommand = ActionCommand.From(ToggleCapture);
            AddBreakConditionCommand = ActionCommand.From(AddBreakCondition, CanAddBreakCondition, false);
            RemoveBreakConditionCommand = ActionCommand.From(RemoveBreakCondition, CanRemoveBreakCondition, false);
            ClearAllBreakpointsCommand = ActionCommand.From(ClearAllBreakpoints, CanClearAllBreakpointsCondition, false);
            ClearEntriesCommand = ActionCommand.From(ClearEntries);
            CopyEntriesCommand = ActionCommand.From(CopyEntries);

            if (IsAutoCapturing)
            {
                ToggleCapture();
            }
        }

        private string _settingsFilePath;
        private string SettingsFilePath
        {
            get
            {
                if (_settingsFilePath == null)
                {
                    var shell = ServiceProvider.GlobalProvider.GetService(typeof(SVsShell)) as IVsShell;
                    object oPath;
                    shell.GetProperty((int)__VSSPROPID4.VSSPROPID_LocalAppDataDir, out oPath);
                    string localVsDataPath = (oPath is string) ? (string)oPath : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    _settingsFilePath =  Path.Combine(localVsDataPath, "AssemblyLoadDebuggerSettings.json");;
                }

                return _settingsFilePath;
            }
        }

        internal void Close()
        {
            _settings?.SaveSettings(SettingsFilePath);

            if (Directory.Exists(_tempDirectory))
            {
                try
                {
                    Directory.Delete(_tempDirectory, recursive: true);
                }
                catch
                {
                }
            }
        }

        private void ClearEntries()
        {
            Events.Clear();
        }

        private void CopyEntries()
        {
            StringBuilder sb = new StringBuilder();
            foreach(string evt in FilteredEvents)
            {
                sb.AppendLine(evt);
                sb.AppendLine(File.ReadAllText($@"{_tempDirectory}\{evt}.txt"));
            }

            if (sb.Length > 0)
            {
                SetClipboardText(sb.ToString());
            }
        }

        private void SetClipboardText(string text)
        {
            int retries = 2;
            while(retries-- > 0)
            {
                try
                {
                    Clipboard.SetText(text);
                    return;
                }
                catch (ExternalException)
                {
                }
                System.Threading.Thread.Sleep(50);
            }
        }

        private bool FilterEvents(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                return true;
            }

            string evt = (string)obj;
            return evt.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) > -1;
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchString)));
                FilteredEvents.Refresh();
            }
        }

        public string CaptureButtonText
        {
            get
            {
                return IsCapturing ? "Stop Capture" : "Capture";
            }
        }

        private bool CanAddBreakCondition()
        {
            return !string.IsNullOrWhiteSpace(UserEntryBreakCondition);
        }

        private bool CanRemoveBreakCondition()
        {
            return SelectedBreakCondition != null;
        }

        private bool CanClearAllBreakpointsCondition()
        {
            return BreakOn?.Count > 0;
        }

        private void AddBreakCondition()
        {
            BreakOn.Add(UserEntryBreakCondition.Trim());
            BreakOn = new List<string>(BreakOn);
            UserEntryBreakCondition = null;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BreakOn)));
        }

        public bool IsCapturing
        {
            get { return _isCapturing; }
            set
            {
                _isCapturing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCapturing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CaptureButtonText)));
            }
        }

        public bool IsAutoCapturing 
        { 
            get
            {
                return _settings.AutoCapture;
            }
            set
            {
                _settings.AutoCapture = value;
            }
        }

        private void RemoveBreakCondition()
        {
            BreakOn.Remove(SelectedBreakCondition);
            BreakOn = new List<string>(BreakOn);
            SelectedBreakCondition = null;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BreakOn)));
        }

        private void ClearAllBreakpoints()
        {
            BreakOn = new List<string>();
            SelectedBreakCondition = null;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BreakOn)));
        }

        public List<string> BreakOn 
        { 
            get
            {
                return _settings.BreakPoints;
            }
            private set
            {
                _settings.BreakPoints = value;
            }
        }

        private void ToggleCapture()
        {
            IsCapturing = !_isCapturing;

            if (_isCapturing)
            {
                AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            }
            else
            {
                AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoaded;
                AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            }
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            BreakIfAssemblyMatches(args.Name);

            string s = _applicationInfo;
            s += $"Managed thread ID: {Thread.CurrentThread.ManagedThreadId}{Environment.NewLine}";
            s += $"Requested name: {args.Name}{Environment.NewLine}";
            s += $"Requesting assembly: {args.RequestingAssembly?.FullName ?? "(Unknown)"}{Environment.NewLine}";

            s += $"Time: {DateTime.Now}{Environment.NewLine}";
            s += new StackTrace(3, true).ToString();
            Application.Current.Dispatcher.BeginInvoke((Action)(() => Events.Add("BIND: " + args.Name)), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            using (Stream str = File.OpenWrite($@"{_tempDirectory}\BIND-{args.Name}.txt"))
            using (StreamWriter w = new StreamWriter(str, Encoding.UTF8, 8192, true))
            {
                w.WriteLine(s);
            }

            return null;
        }

        [DebuggerStepThrough]
        private void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
        {
            BreakIfAssemblyMatches(args.LoadedAssembly.FullName);

            AssemblyName name = args.LoadedAssembly.GetName();
            string s = _applicationInfo;
            s += $"Managed thread ID: {Thread.CurrentThread.ManagedThreadId}{Environment.NewLine}";
            s += $"Image: {args.LoadedAssembly.FullName}{Environment.NewLine}";
            s += $"Codebase: {args.LoadedAssembly.CodeBase}{Environment.NewLine}";
            s += $"Location: {args.LoadedAssembly.Location}{Environment.NewLine}";
            s += $"Name: {name.Name}{Environment.NewLine}";
            s += $"Version: {name.Version}{Environment.NewLine}";
            s += $"Architecture: {name.ProcessorArchitecture}{Environment.NewLine}";
            s += $"Culture: {name.CultureName}{Environment.NewLine}";

            byte[] publicKey = name.GetPublicKey();
            if (publicKey != null)
            {
                string hex = string.Join("", publicKey.Select(x => x.ToString("X2")));
                s += $"Public Key: {hex}{Environment.NewLine}";
                s += $"Hash Algorithm: {name.HashAlgorithm}{Environment.NewLine}";
            }

            s += $"Image Runtime Version: {args.LoadedAssembly.ImageRuntimeVersion}{Environment.NewLine}";
            s += $"Flags: {name.Flags}{Environment.NewLine}";
            s += $"Is Dynamic: {args.LoadedAssembly.IsDynamic}{Environment.NewLine}";
            s += $"Is Fully Trusted: {args.LoadedAssembly.IsFullyTrusted}{Environment.NewLine}";
            s += $"Reflection Only: {args.LoadedAssembly.ReflectionOnly}{Environment.NewLine}";
            s += $"Time: {DateTime.Now}{Environment.NewLine}";
            s += new StackTrace(3, true).ToString();
            Application.Current.Dispatcher.BeginInvoke((Action)(() => Events.Add(args.LoadedAssembly.FullName)), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            using (Stream str = File.OpenWrite($@"{_tempDirectory}\{args.LoadedAssembly.FullName}.txt"))
            using (StreamWriter w = new StreamWriter(str, Encoding.UTF8, 8192, true))
            {
                w.WriteLine(s);
            }
        }

        private void BreakIfAssemblyMatches(string assemblyName)
        {
            if (BreakOn.Any(x =>
            {
                try
                {
                    return Regex.IsMatch(assemblyName, x, RegexOptions.IgnoreCase);
                }
                catch
                {
                    return false;
                }
            }))
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                else
                {
                    Debugger.Launch();
                }
            }
        }

        public ObservableCollection<string> Events { get; }

        public static AssemblyLoadDebuggerControlViewModel Instance { get; } = new AssemblyLoadDebuggerControlViewModel();

        public ICommand ToggleCaptureCommand { get; }

        public ICommand OpenLogCommand { get; }

        public ICommand AddBreakConditionCommand { get; }

        public ICommand RemoveBreakConditionCommand { get; }

        public ICommand ClearAllBreakpointsCommand { get; }
        
       public ICommand ClearEntriesCommand { get; }

        public ICommand CopyEntriesCommand { get; }

        public ICollectionView FilteredEvents { get; }

        public string SelectedBreakCondition
        {
            get { return _selectedBreakCondition; }
            set
            {
                _selectedBreakCondition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBreakCondition)));
                RemoveBreakConditionCommand.CanExecute(value);
            }
        }

        public string UserEntryBreakCondition
        {
            get { return _userEntryBreakCondition; }
            set
            {
                _userEntryBreakCondition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserEntryBreakCondition)));
                AddBreakConditionCommand.CanExecute(value);
            }
        }
    }
}
