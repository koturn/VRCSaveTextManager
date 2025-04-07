using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Koturn.VRChat.Log;
using Koturn.VRChat.Log.Events;


namespace VRCSaveTextManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log watcher.
        /// </summary>
        private readonly VRCExLogWatcher _logWatcher = new();

        /// <summary>
        /// Initialize components.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _bulletTimeAgentSaveControl.MainWindow = this;
            _idleCubeSaveControl.MainWindow = this;
            _idleDefenseSaveControl.MainWindow = this;
            _idleHomeSaveControl.MainWindow = this;
            _magicalCursedLandSaveControl.MainWindow = this;
            _rhapsodySaveControl.MainWindow = this;
            _terrosOfNowhereSaveControl.MainWindow = this;
        }

        /// <summary>
        /// Parse specified log files.
        /// </summary>
        /// <param name="filePaths">Log file paths.</param>
        /// <returns>Parsing task.</returns>
        private async Task ParseLogFilesAsync(string[] filePaths)
        {
            _buttonLoadLogFile.IsEnabled = false;
            _buttonVacuumAnalyze.IsEnabled = false;
            try
            {
                await StartParsingLogFiles(filePaths);
                _bulletTimeAgentSaveControl.RefreshListView();
                _idleCubeSaveControl.RefreshListView();
                _idleDefenseSaveControl.RefreshListView();
                _idleHomeSaveControl.RefreshListView();
                _magicalCursedLandSaveControl.RefreshListView();
                _rhapsodySaveControl.RefreshListView();
                _terrosOfNowhereSaveControl.RefreshListView();
            }
            finally
            {
                _buttonVacuumAnalyze.IsEnabled = true;
                _buttonLoadLogFile.IsEnabled = true;
            }
        }

        /// <summary>
        /// Start new log file parsing task.
        /// </summary>
        /// <param name="filePaths">Log file paths.</param>
        /// <returns>Parsing task.</returns>
        private Task StartParsingLogFiles(string[] filePaths)
        {
            return Task.Factory.StartNew(param =>
            {
                var filePaths = (string[])param!;
                for (int i = 0; i < filePaths.Length; i++)
                {
                    var filePath = filePaths[i];
                    var fileName = Path.GetFileName(filePath);

                    _statusLabel.Dispatcher.Invoke(() => _statusLabel.Content = $"Parsing {fileName} ... ({i + 1} / {filePaths.Length})");
                    using (var logParser = new VRCExLogParser(filePath))
                    {
                        logParser.BulletTimeAgentSaved += LogParser_BulletTimeAgentSaved;
                        logParser.IdleCubeSaved += LogParser_IdleCubeSaved;
                        logParser.IdleDefenseSaved += LogParser_IdleDefenseSaved;
                        logParser.IdleHomeSaved += LogParser_IdleHomeSaved;
                        logParser.MagicalCursedLandSaved += LogParser_MagicalCursedLandSaved;
                        logParser.RhapsodySaved += LogParser_RhapsodySaved;
                        logParser.TerrorsOfNowhereSaved += LogParser_TerrorsOfNowhereSaved;

                        var sw = Stopwatch.StartNew();
                        Console.WriteLine($"Parse: {filePath} ...");
                        logParser.Parse();
                        Console.WriteLine($"Parse: {filePath} ... Done; elapsed {sw.ElapsedMilliseconds} ms");

                        UpdateLogFile(logParser.LogFrom, logParser.LogUntil, fileName);
                    }
                }
                _statusLabel.Dispatcher.Invoke(() => _statusLabel.Content = null);
            }, filePaths);
        }

        /// <summary>
        /// VACUUM and ANALYZE all databases.
        /// </summary>
        /// <returns>VACUUM and ANALYZE task.</returns>
        private Task StartVacuumAnalyzeDatabases()
        {
            return Task.Run(() =>
            {
                var dbNames = new[] {
                    DatabaseNames.BulletTimeAgent,
                    DatabaseNames.IdleCube,
                    DatabaseNames.IdleDefense,
                    DatabaseNames.IdleHome,
                    DatabaseNames.MagicalCursedLand,
                    DatabaseNames.Rhapsody,
                    DatabaseNames.TerrorsOfNowhere
                };
                for (int i = 0; i < dbNames.Length; i++)
                {
                    var dbName = dbNames[i];
                    var executor = App.GetSqlExecutor(dbName);
                    _statusLabel.Dispatcher.Invoke(() => _statusLabel.Content = $"VACUUM {dbName}.db ... ({i + 1} / {dbNames.Length})");
                    executor.Vacuum();
                    _statusLabel.Dispatcher.Invoke(() => _statusLabel.Content = $"ANALYZE {dbName}.db ... ({i + 1} / {dbNames.Length})");
                    executor.Analyze();
                }
                _statusLabel.Dispatcher.Invoke(() => _statusLabel.Content = null);
            });
        }

        /// <summary>
        /// Occurs when the input system reports an underlying drag event with this element as the potential drop target.
        /// </summary>
        /// <param name="sender"><see cref="MainWindow"/> instance, "this".</param>
        /// <param name="e">The event data.</param>
        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = true;
        }

        /// <summary>
        /// Occurs when the input system reports an underlying drop event with this element as the drop target.
        /// </summary>
        /// <param name="sender"><see cref="MainWindow"/> instance, "this".</param>
        /// <param name="e">The event data.</param>
        private async void Window_DropAsync(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) || !_buttonLoadLogFile.IsEnabled)
            {
                return;
            }

            await ParseLogFilesAsync((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        /// <summary>
        /// Occurs when a key is pressed while focus is on this element.
        /// </summary>
        /// <param name="sender"><see cref="MainWindow"/> instance, "this".</param>
        /// <param name="e">The event data.</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                App.ToggleConsole();
            }
        }

        /// <summary>
        /// Start parsing existing VRChat log files.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private async void LoadLogButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await ParseLogFilesAsync(VRCBaseLogParser.GetLogFilePaths());
        }

        private async void ButtonVacuumAnalyze_ClickAsync(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            _buttonLoadLogFile.IsEnabled = false;
            button.IsEnabled = false;
            try
            {
                await StartVacuumAnalyzeDatabases();
            }
            finally
            {
                _buttonVacuumAnalyze.IsEnabled = true;
                _buttonLoadLogFile.IsEnabled = true;
            }
        }

        /// <summary>
        /// Setup evnet handler of <see cref="_logWatcher"/> and start it.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start watching log file ...");
            var logWatcher = _logWatcher;
            logWatcher.BulletTimeAgentSaved += LogWatcher_BulletTimeAgentSaved;
            logWatcher.IdleCubeSaved += LogWatcher_IdleCubeSaved;
            logWatcher.IdleDefenseSaved += LogWatcher_IdleDefenseSaved;
            logWatcher.IdleHomeSaved += LogWatcher_IdleHomeSaved;
            logWatcher.MagicalCursedLandSaved += LogWatcher_MagicalCursedLandSaved;
            logWatcher.RhapsodySaved += LogWatcher_RhapsodySaved;
            logWatcher.TerrorsOfNowhereSaved += LogWatcher_TerrorsOfNowhereSaved;
            logWatcher.FileClosed += LogWatcher_FileClosed;
            logWatcher.Start();
            Console.WriteLine("Start watching log file ... Started");
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Bullet Time Agent is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_BulletTimeAgentSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.BulletTimeAgent, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Cube is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_IdleCubeSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.IdleCube, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Defense is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_IdleDefenseSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.IdleDefense, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Home is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_IdleHomeSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.IdleHome, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Magical Cursed Land is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_MagicalCursedLandSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.MagicalCursedLand, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Rhapsody is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_RhapsodySaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.Rhapsody, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Terrors of Nowhere is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogParser_TerrorsOfNowhereSaved(object sender, SaveEventArgs e)
        {
            var logParser = (VRCExLogParser)sender;
            UpsertSave(DatabaseNames.TerrorsOfNowhere, e.LogAt, e.SaveText, logParser.LogFrom, logParser.LogUntil, GetFileName(logParser));
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Bullet Time Agent is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogParser"/> instance.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_BulletTimeAgentSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.BulletTimeAgent, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _bulletTimeAgentSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Cube is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_IdleCubeSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.IdleCube, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _idleCubeSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Defense is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_IdleDefenseSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.IdleDefense, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _idleDefenseSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Idle Home is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_IdleHomeSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.IdleHome, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _idleHomeSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Magical Cursed Land is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_MagicalCursedLandSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.MagicalCursedLand, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _magicalCursedLandSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Rhapsody is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_RhapsodySaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.Rhapsody, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _rhapsodySaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when detect a log that save data text of Terrors of Nowhere is generated.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains save text data.</param>
        private void LogWatcher_TerrorsOfNowhereSaved(object sender, SaveEventArgs e)
        {
            var logWatcher = (VRCExLogWatcher)sender;
            UpsertSave(DatabaseNames.TerrorsOfNowhere, e.LogAt, e.SaveText, logWatcher.CurrentLogFrom, logWatcher.CurrentLogUntil, GetFileName(logWatcher));
            _terrosOfNowhereSaveControl.RefreshListView();
        }

        /// <summary>
        /// Occurs when a log file closed.
        /// </summary>
        /// <param name="sender"><see cref="VRCExLogWatcher"/> instance, <see cref="_logWatcher"/>.</param>
        /// <param name="e">The event data that contains closed log file information.</param>
        private void LogWatcher_FileClosed(object sender, FileCloseEventArgs e)
        {
            UpdateLogFile(e.LogFrom, e.LogUntil, Path.GetFileName(e.FilePath));
        }

        /// <summary>
        /// Stop watching log file.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Stop watching log file ...");
            _logWatcher.Stop();
            Console.WriteLine("Stop watching log file ... Done");
        }


        /// <summary>
        /// INSERT or UPDATE t_save_text and t_logfile.
        /// </summary>
        /// <param name="dbName">Database name.</param>
        /// <param name="logAt">Save log timestamp.</param>
        /// <param name="saveText">Save data text.</param>
        /// <param name="logFrom">First timestamp of the log file.</param>
        /// <param name="logUntil">Last timestamp of the log file..</param>
        /// <param name="logFileName">Log file name.</param>
        private static void UpsertSave(string dbName, DateTime logAt, string saveText, DateTime logFrom, DateTime logUntil, string logFileName)
        {
            var executor = App.GetSqlExecutor(dbName);
            using (var transaction = executor.BeginTransaction())
            {
                var count = executor.UpsertSave(logAt, saveText, logFrom);

                if (count > 0)
                {
                    executor.UpsertLogFile(logFrom, logUntil, logFileName);
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// UPDATE t_save_text.
        /// </summary>
        /// <param name="logFrom">First timestamp of the log file.</param>
        /// <param name="logUntil">Last timestamp of the log file..</param>
        /// <param name="logFileName">Log file name.</param>
        private static void UpdateLogFile(DateTime logFrom, DateTime logUntil, string logFileName)
        {
            foreach (var executor in App.GetAllSqlExecutor())
            {
                using (var transaction = executor.BeginTransaction())
                {
                    executor.UpdateLogFile(logFrom, logUntil, logFileName);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Get underlying file name from specified <see cref="VRCBaseLogParser"/>.
        /// </summary>
        /// <param name="logParser">VRChat log parser.</param>
        /// <returns>Underlying file name of <see cref="VRCBaseLogParser"/>.</returns>
        private static string GetFileName(VRCBaseLogParser logParser)
        {
            var filePath = logParser.FilePath;
            return filePath == null ? string.Empty : Path.GetFileName(filePath);
        }

        /// <summary>
        /// Get underlying file name from specified <see cref="VRCBaseLogWatcher"/>.
        /// </summary>
        /// <param name="logWatcher">VRChat log watcher.</param>
        /// <returns>Underlying file name of <see cref="VRCBaseLogWatcher"/>.</returns>
        private static string GetFileName(VRCBaseLogWatcher logWatcher)
        {
            var filePath = logWatcher.CurrentFilePath;
            return filePath == null ? string.Empty : Path.GetFileName(filePath);
        }
    }
}
