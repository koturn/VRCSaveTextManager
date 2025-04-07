using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VRCSaveTextManager.Internals.Extensions;


namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for BaseSaveTextControl.xaml
    /// </summary>
    public abstract partial class BaseSaveTextControl : UserControl
    {
        /// <summary>
        /// Referenct to main window.
        /// </summary>
        public MainWindow? MainWindow { get; set; }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public BaseSaveTextControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Refresh <see cref="ListBox"/> of log files.
        /// </summary>
        public void RefreshListView()
        {
            var executor = GetSqlExecutor();
            if (executor == null)
            {
                return;
            }

            Dispatcher.InvokeIfNeeded(() =>
            {
                var items = _logFileListBox.Items;
                items.Clear();
                foreach (var saveFileName in executor.SelectLogFile())
                {
                    items.Add(saveFileName);
                }

                if (items.Count > 0)
                {
                    _logFileListBox.SelectedIndex = 0;
                }
            });
        }


        /// <summary>
        /// Try to get <see cref="SqlExecutor"/> instance.
        /// </summary>
        /// <returns><see cref="SqlExecutor"/> instance.</returns>
        protected abstract SqlExecutor? GetSqlExecutor();

        /// <summary>
        /// SELECT log file names.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of log file names.</returns>
        protected IEnumerable<string> SelectLogFile()
        {
            var executor = GetSqlExecutor();
            return executor == null ? [] : executor.SelectLogFile();
        }

        /// <summary>
        /// SELECT timestamp of specified log file.
        /// </summary>
        /// <param name="logFileName">Log file name.</param>
        /// <returns>Timestamp of specified log file.</returns>
        protected IEnumerable<DateTime> SelectSaveTimestamp(string logFileName)
        {
            var executor = GetSqlExecutor();
            return executor == null ? [] : executor.SelectSaveTimestamp(logFileName);
        }

        /// <summary>
        /// SELECT one save data text.
        /// </summary>
        /// <param name="logAt">Log timestamp.</param>
        /// <returns>Save data text.</returns>
        protected string? SelectOneSave(DateTime logAt)
        {
            return GetSqlExecutor()?.SelectOneSave(logAt);
        }


        /// <summary>
        /// Occurs when the value of the <see cref="UIElement.IsVisible"/> property changes.
        /// </summary>
        /// <param name="sender"><see cref="BaseSaveTextControl"/> instance, "this".</param>
        /// <param name="e">The event data.</param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                RefreshListView();
                ((UserControl)sender).IsVisibleChanged -= UserControl_IsVisibleChanged;
            }
        }

        /// <summary>
        /// <para>Occurs when the selection of a <see cref="System.Windows.Controls.Primitives.Selector"/> changes.</para>
        /// <para>Load log file names and select latest one.</para>
        /// </summary>
        /// <param name="sender"><see cref="ListBox"/> instance, <see cref="_logFileListBox"/></param>
        /// <param name="e">The event data.</param>
        private void LogFileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var selectedText = (string)listBox.SelectedItem;
            if (selectedText == null)
            {
                return;
            }

            var items = _saveDateTimeListBox.Items;
            items.Clear();
            foreach (var logAt in SelectSaveTimestamp(selectedText))
            {
                items.Add(logAt);
            }

            if (items.Count > 0)
            {
                _saveDateTimeListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// <para>Occurs when the selection of a <see cref="System.Windows.Controls.Primitives.Selector"/> changes.</para>
        /// <para>Load save text timestamps and select latest one.</para>
        /// </summary>
        /// <param name="sender"><see cref="ListBox"/> instance.</param>
        /// <param name="e">The event data.</param>
        private void SaveDateTimeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItem == null)
            {
                return;
            }
            var dtSave = (DateTime)listBox.SelectedItem;
            dtSave += TimeZoneInfo.Local.GetUtcOffset(dtSave);

            _saveTextBox.Text = SelectOneSave(dtSave);
        }

        /// <summary>
        /// <para>Occurs when a "Copy to Clipboard" Button is clicked.</para>
        /// <para>Copy save data text to the clipboard.</para>
        /// </summary>
        /// <param name="sender">"Copy to Clipboard" Button.</param>
        /// <param name="e">The event data.</param>
        private void CopyToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            var saveText = _saveTextBox.Text;
            if (saveText == string.Empty)
            {
                return;
            }
            Clipboard.SetData(DataFormats.Text, saveText);

            var dtSave = (DateTime)_saveDateTimeListBox.SelectedItem;
            MessageBox.Show(
                MainWindow,
                $"Copy save data text at {dtSave:yyyy-mm-dd HH:mm:ss}; {saveText.Length} chars",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
