using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public AssemblyLoadDebuggerControlViewModel()
        {
            Events = new ObservableCollection<string>();
            FilteredEvents = CollectionViewSource.GetDefaultView(Events);
            FilteredEvents.Filter = FilterEvents;
            BreakOn = new List<string>();

            string tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Directory.CreateDirectory(tempFile);
            _tempDirectory = tempFile;
            
            OpenLogCommand = ActionCommand.From<string>(file =>
            {
                DTE2 dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE2;
                dte?.ItemOperations?.OpenFile($@"{_tempDirectory}\{file}.txt");
            });

            ToggleCaptureCommand = ActionCommand.From(ToggleCapture);
            AddBreakConditionCommand = ActionCommand.From(AddBreakCondition, CanAddBreakCondition, false);
            RemoveBreakConditionCommand = ActionCommand.From(RemoveBreakCondition, CanRemoveBreakCondition, false);
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

        private bool CanAddBreakCondition()
        {
            return !string.IsNullOrWhiteSpace(UserEntryBreakCondition);
        }

        private bool CanRemoveBreakCondition()
        {
            return SelectedBreakCondition != null;
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
            }
        }

        private void RemoveBreakCondition()
        {
            BreakOn.Remove(SelectedBreakCondition);
            BreakOn = new List<string>(BreakOn);
            SelectedBreakCondition = null;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BreakOn)));
        }

        public List<string> BreakOn { get; private set; }

        private void ToggleCapture()
        {
            IsCapturing = !_isCapturing;

            if (_isCapturing)
            {
                AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
            }
            else
            {
                AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoaded;
            }
        }

        [DebuggerStepThrough]
        private void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
        {
            if (BreakOn.Any(x =>
            {
                try
                {
                    return Regex.IsMatch(args.LoadedAssembly.FullName, x);
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

            string s = string.Empty;
            s += $"Window Title: {Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion}{Environment.NewLine}";
            s += $"Main Module File: {Process.GetCurrentProcess().MainModule.FileName}{Environment.NewLine}";
            s += $"Main Module Version: {Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion}{Environment.NewLine}";
            s += $"Managed thread ID: {Thread.CurrentThread.ManagedThreadId}{Environment.NewLine}";
            s += $"Time: {DateTime.Now}{Environment.NewLine}";
            s += new StackTrace(3, true).ToString();
            Application.Current.Dispatcher.BeginInvoke((Action)(() => Events.Add(args.LoadedAssembly.FullName)), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            using (Stream str = File.OpenWrite($@"{_tempDirectory}\{args.LoadedAssembly.FullName}.txt"))
            using (StreamWriter w = new StreamWriter(str, Encoding.UTF8, 8192, true))
            {
                w.WriteLine(s);
            }
        }

        public ObservableCollection<string> Events { get; }

        public static AssemblyLoadDebuggerControlViewModel Instance { get; } = new AssemblyLoadDebuggerControlViewModel();

        public ICommand ToggleCaptureCommand { get; }

        public ICommand OpenLogCommand { get; }

        public ICommand AddBreakConditionCommand { get; }

        public ICommand RemoveBreakConditionCommand { get; }

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
