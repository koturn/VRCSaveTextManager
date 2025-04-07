namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for IdleDefenseSaveControl.xaml
    /// </summary>
    public sealed class IdleDefenseSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.IdleDefense);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public IdleDefenseSaveControl()
        {
            InitializeComponent();
        }
    }
}
