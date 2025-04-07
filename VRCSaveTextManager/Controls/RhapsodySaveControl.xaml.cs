namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for RhapsodySaveControl.xaml
    /// </summary>
    public sealed class RhapsodySaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.Rhapsody);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public RhapsodySaveControl()
        {
            InitializeComponent();
        }
    }
}
