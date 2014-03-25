namespace CodeAnalysis.ViewModels
{
    using Microsoft.Win32;

    public class ViewModel : BaseViewModel
    {
        #region Constructors

        public ViewModel()
        {
            BrowseTrunkMetricsFileCommand = new RelayCommand(param => BrowseMetricsFile(true));
            BrowseBrancheMetricsFileCommand = new RelayCommand(param => BrowseMetricsFile(false));
            ProceedCommand = new RelayCommand(param => Proceed());
        }

        #endregion

        #region Commands

        public RelayCommand BrowseTrunkMetricsFileCommand { get; set; }
        public RelayCommand BrowseBrancheMetricsFileCommand { get; set; }
        public RelayCommand ProceedCommand { get; set; }

        #endregion

        #region Properties

        private string trunkMetricsFilePath;

        public string TrunkMetricsFilePath
        {
            get { return trunkMetricsFilePath; }
            set { trunkMetricsFilePath = value; OnPropertyChanged("TrunkMetricsFilePath"); }
        }

        private string brancheMetricsFilePath;

        public string BrancheMetricsFilePath
        {
            get { return brancheMetricsFilePath; }
            set { brancheMetricsFilePath = value; OnPropertyChanged("BrancheMetricsFilePath"); }
        }

        #endregion

        #region Methods

        private void BrowseMetricsFile(bool isTrunk)
        {
            var dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                if (isTrunk)
                {
                    TrunkMetricsFilePath = dialog.FileName;
                }
                else
                {
                    BrancheMetricsFilePath = dialog.FileName;
                }
            }
        }

        private void Proceed()
        {
            if (!string.IsNullOrWhiteSpace(TrunkMetricsFilePath) && !string.IsNullOrWhiteSpace(BrancheMetricsFilePath))
            {
            }
        }

        #endregion
    }
}