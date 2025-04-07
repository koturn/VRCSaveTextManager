namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for TerrorsOfNowhereSaveControl.xaml
    /// </summary>
    public sealed class TerrorsOfNowhereSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.TerrorsOfNowhere);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public TerrorsOfNowhereSaveControl()
        {
            InitializeComponent();
        }
    }
}
