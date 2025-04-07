using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if NET9_0_OR_GREATER
using System.Threading;
#endif  // NET9_0_OR_GREATER
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SQLitePCL;
using Koturn.Windows.Consoles;


namespace VRCSaveTextManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Lock object for exception log.
        /// </summary>
#if NET9_0_OR_GREATER
        private static readonly Lock _lockLog = new();
#else
        private static readonly object _lockLog = new();
#endif  // NET9_0_OR_GREATER
        /// <summary>
        /// Console state.
        /// </summary>
        private static ConsoleState _consoleState;

        /// <summary>
        /// Database directory.
        /// </summary>
        private static readonly string LocalLowDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "VRCSaveTextManager");

        /// <summary>
        /// Connection cache.
        /// </summary>
        private static readonly Dictionary<string, SqlExecutor> _executorDict = [];

        /// <summary>
        /// Register callback methods to unhandled exception handlers and start WPF application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

#if DEBUG
            SetupConsole();
#else
            foreach (var arg in args)
            {
                if (arg == "--debug")
                {
                    SetupConsole();
                    break;
                }
            }
#endif

            Batteries.Init();

            var app = new App();
            app.InitializeComponent();
            app.Run();

            DisposeAllSqlExecutor();
        }


        /// <summary>
        /// Setup <see cref="Application.DispatcherUnhandledException"/> to catch unhandled exception in UI thread.
        /// </summary>
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }


        /// <summary>
        /// <para>Get <see cref="SqlExecutor"/>.</para>
        /// <para>If database file is not exists or the file size is zero, returns null.</para>
        /// </summary>
        /// <param name="name">Database name.</param>
        /// <returns><see cref="SqlExecutor"/> instance.</returns>
        public static SqlExecutor? GetSqlExecutorForRead(string name)
        {
            if (_executorDict.TryGetValue(name, out var executor))
            {
                return executor;
            }

            var dbFilePath = Path.Combine(LocalLowDirectory, name + ".db");
            if (!File.Exists(dbFilePath) || new FileInfo(dbFilePath).Length == 0)
            {
                return null;
            }

            executor = new SqlExecutor("Data Source=" + dbFilePath);

            _executorDict.Add(name, executor);

            return executor;
        }

        /// <summary>
        /// <para>Get <see cref="SqlExecutor"/>.</para>
        /// <para>If database file is not exists or the file size is zero, (re-)create database and create <see cref="SqlExecutor"/>.</para>
        /// </summary>
        /// <param name="name">Database name.</param>
        /// <returns><see cref="SqlExecutor"/> instance.</returns>
        public static SqlExecutor GetSqlExecutor(string name)
        {
            if (_executorDict.TryGetValue(name, out var executor))
            {
                return executor;
            }

            if (!Directory.Exists(LocalLowDirectory))
            {
                Directory.CreateDirectory(LocalLowDirectory);
            }

            var dbFileName = name + ".db";
            var dbFilePath = Path.Combine(LocalLowDirectory, dbFileName);

            var isCreateNew = false;
            if (File.Exists(dbFilePath))
            {
                if (new FileInfo(dbFilePath).Length == 0)
                {
                    File.Delete(dbFilePath);
                    isCreateNew = true;
                }
            }
            else
            {
                isCreateNew = true;
            }

            executor = new SqlExecutor("Data Source=" + dbFilePath);
            if (isCreateNew)
            {
                executor.InitializeDatabase();
                executor.CreateViews();
            }

            _executorDict.Add(name, executor);

            return executor;
        }

        /// <summary>
        /// Dispose all cached <see cref="SqlExecutor"/>.
        /// </summary>
        public static void DisposeAllSqlExecutor()
        {
            foreach (var executor in _executorDict.Values)
            {
                executor.Dispose();
            }
            _executorDict.Clear();
        }

        /// <summary>
        /// Get all created SQL executor.
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="SqlExecutor"/>.</returns>
        public static List<SqlExecutor> GetAllSqlExecutor()
        {
            return [.. _executorDict.Values];
        }

        /// <summary>
        /// Setup console when console is not allocated.
        /// </summary>
        public static void EnsureSetupConsole()
        {
            if (_consoleState == ConsoleState.NotAlloced)
            {
                SetupConsole();
            }
        }

        /// <summary>
        /// Setup console; allocate and disable close buttons and close keys, change output encoding to UTF-8.
        /// </summary>
        public static void SetupConsole()
        {
            ConsoleUtil.AllocConsole();
            ConsoleUtil.DisableCloseButton();
            ConsoleUtil.DisableExitKeys();
            Console.OutputEncoding = Encoding.UTF8;
            _consoleState = ConsoleState.Show;
        }

        /// <summary>
        /// Toggle the console to show or hide.
        /// </summary>
        public static void ToggleConsole()
        {
            switch (_consoleState)
            {
                case ConsoleState.NotAlloced:
                    SetupConsole();
                    break;
                case ConsoleState.Show:
                    ConsoleUtil.HideConsole();
                    _consoleState = ConsoleState.Hide;
                    break;
                case ConsoleState.Hide:
                    ConsoleUtil.ShowConsole();
                    _consoleState = ConsoleState.Show;
                    break;
            }
        }

        /// <summary>
        /// Callback method for unhandled exceptions in UI thread.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;

            WriteExceptionLog(ex);

            var result = MessageBox.Show(
                $"An exception occured at {ex.TargetSite?.Name}.\n"
                    + "Are you sure want to continue program?\n"
                    + $"{ex.Message}",
                ex.GetType().FullName,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Callback method for unhandled exceptions in <see cref="Task"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            var ex = e.Exception.InnerException;
            if (ex == null)
            {
                return;
            }

            WriteExceptionLog(ex);

            var result = MessageBox.Show(
                $"An exception occured on background task {ex.TargetSite?.Name}."
                    + "Are you sure want to continue program?\n"
                    + $"{ex.Message}",
                ex.GetType().FullName,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                e.SetObserved();
            }
        }

        /// <summary>
        /// Callback method for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is not Exception ex)
            {
                MessageBox.Show("Unhandled exception is not treat as System.Exception.");
                return;
            }

            WriteExceptionLog(ex);

            MessageBox.Show(
                $"An exception occured on background task {ex.TargetSite?.Name}."
                    + "Terminate this program\n"
                    + $"{ex.Message}",
                ex.GetType().FullName,
                MessageBoxButton.OK,
                MessageBoxImage.Stop);

            Environment.Exit(0);
        }

        /// <summary>
        /// Write exception log to UnhandledException.log.
        /// </summary>
        /// <param name="ex">An exception.</param>
        private static void WriteExceptionLog(Exception ex)
        {
            lock (_lockLog)
            {
                using (var fs = new FileStream("WindowResizerException.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff)}] {ex}");
                }
            }
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff)}] {ex}");
        }

        /// <summary>
        /// Console state values.
        /// </summary>
        private enum ConsoleState
        {
            /// <summary>
            /// Not allocated.
            /// </summary>
            NotAlloced,
            /// <summary>
            /// Console is shown.
            /// </summary>
            Show,
            /// <summary>
            /// Console is hide.
            /// </summary>
            Hide
        }
    }
}
