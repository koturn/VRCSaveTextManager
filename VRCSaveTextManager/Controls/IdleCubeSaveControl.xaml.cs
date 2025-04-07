namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for IdleCubeSaveControl.xaml
    /// </summary>
    public sealed class IdleCubeSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.IdleCube);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public IdleCubeSaveControl()
        {
            InitializeComponent();
        }
    }
}
