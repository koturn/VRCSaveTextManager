namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for BulletTimeAgentSaveControl.xaml
    /// </summary>
    public sealed class BulletTimeAgentSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.BulletTimeAgent);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public BulletTimeAgentSaveControl()
        {
            InitializeComponent();
        }
    }
}
