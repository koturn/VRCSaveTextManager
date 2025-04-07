namespace VRCSaveTextManager.Controls
{
    /// <summary>
    /// Interaction logic for MagicalCursedLandSaveControl.xaml
    /// </summary>
    public sealed class MagicalCursedLandSaveControl : BaseSaveTextControl
    {
        /// <inheritdoc/>
        protected override SqlExecutor? GetSqlExecutor()
        {
            return App.GetSqlExecutorForRead(DatabaseNames.MagicalCursedLand);
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        public MagicalCursedLandSaveControl()
        {
            InitializeComponent();
        }
    }
}
