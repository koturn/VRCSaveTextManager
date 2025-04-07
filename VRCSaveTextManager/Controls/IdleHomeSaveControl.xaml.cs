namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for IdleHomeSaveControl.xaml
    /// </summary>
    public sealed class IdleHomeSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.IdleHome);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public IdleHomeSaveControl()
        {
            InitializeComponent();
        }
    }
}
