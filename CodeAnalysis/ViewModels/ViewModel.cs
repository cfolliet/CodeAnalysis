namespace CodeAnalysis.ViewModels
{
    using System.Collections.ObjectModel;
    using System.IO;

    using CodeAnalysis.BusinessLogic;
    using CodeAnalysis.Models;

    using Microsoft.Win32;

    public class ViewModel : BaseViewModel
    {
        #region Constructors

        public ViewModel()
        {
            BrowseTrunkMetricsFileCommand = new RelayCommand(param => BrowseMetricsFile(FileType.TrunkMetrics));
            BrowseBrancheMetricsFileCommand = new RelayCommand(param => BrowseMetricsFile(FileType.BrancheMetrics));
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

        private ObservableCollection<CodeMetricsLineView> tree;

        public ObservableCollection<CodeMetricsLineView> Tree
        {
            get { return tree; }
            set { tree = value; OnPropertyChanged("Tree"); }
        }

        #endregion

        #region Enum

        private enum FileType
        {
            TrunkMetrics,
            BrancheMetrics,
            TrunkCoverage,
            BrancheCoverage,
        }

        #endregion

        #region Methods

        private void BrowseMetricsFile(FileType type)
        {
            var dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                switch (type)
                {
                    case FileType.TrunkMetrics:
                        TrunkMetricsFilePath = dialog.FileName;
                        break;

                    case FileType.BrancheMetrics:
                        BrancheMetricsFilePath = dialog.FileName;
                        break;
                }
            }
        }

        private void Proceed()
        {
            if (!string.IsNullOrWhiteSpace(TrunkMetricsFilePath) && !string.IsNullOrWhiteSpace(BrancheMetricsFilePath))
            {
                Stream codeMetricsTrunkExcel = new FileStream(TrunkMetricsFilePath, FileMode.Open);
                Stream codeMetricsBrancheExcel = new FileStream(BrancheMetricsFilePath, FileMode.Open);

                var tmp = CodeMetricsGenerator.Generate(codeMetricsTrunkExcel, codeMetricsBrancheExcel);

                codeMetricsTrunkExcel.Close();
                codeMetricsBrancheExcel.Close();

                Tree = new ObservableCollection<CodeMetricsLineView>(tmp);
            }
        }

        #endregion
    }
}