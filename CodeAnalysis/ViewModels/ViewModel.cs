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
            BrowseTrunkMetricsFileCommand = new RelayCommand(param => BrowseFiles(FileType.TrunkMetrics));
            BrowseBrancheMetricsFileCommand = new RelayCommand(param => BrowseFiles(FileType.BrancheMetrics));
            ProceedMetricsCommand = new RelayCommand(param => ProceedMetrics());

            BrowseTrunkCoverageFileCommand = new RelayCommand(param => BrowseFiles(FileType.TrunkCoverage));
            BrowseBrancheCoverageFileCommand = new RelayCommand(param => BrowseFiles(FileType.BrancheCoverage));
            ProceedCoverageCommand = new RelayCommand(param => ProceedCoverage());
        }

        #endregion

        #region Commands

        public RelayCommand BrowseTrunkMetricsFileCommand { get; set; }
        public RelayCommand BrowseBrancheMetricsFileCommand { get; set; }
        public RelayCommand ProceedMetricsCommand { get; set; }

        public RelayCommand BrowseTrunkCoverageFileCommand { get; set; }
        public RelayCommand BrowseBrancheCoverageFileCommand { get; set; }
        public RelayCommand ProceedCoverageCommand { get; set; }

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

        private ObservableCollection<CodeMetricsLineView> metricsTree;

        public ObservableCollection<CodeMetricsLineView> MetricsTree
        {
            get { return metricsTree; }
            set { metricsTree = value; OnPropertyChanged("MetricsTree"); }
        }

        private string trunkCoverageFilePath;

        public string TrunkCoverageFilePath
        {
            get { return trunkCoverageFilePath; }
            set { trunkCoverageFilePath = value; OnPropertyChanged("TrunkCoverageFilePath"); }
        }

        private string brancheCoverageFilePath;

        public string BrancheCoverageFilePath
        {
            get { return brancheCoverageFilePath; }
            set { brancheCoverageFilePath = value; OnPropertyChanged("BrancheCoverageFilePath"); }
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

        private void BrowseFiles(FileType type)
        {
            var dialog = new OpenFileDialog();

            switch (type)
            {
                case FileType.TrunkMetrics:
                case FileType.BrancheMetrics:
                    dialog.Title = "Open a code metrics file";
                    dialog.Filter = "Code Metrics Files|*.xlsx";
                    break;

                case FileType.TrunkCoverage:
                case FileType.BrancheCoverage:
                    dialog.Title = "Open a code coverage file";
                    dialog.Filter = "Code Coverage Files|*.coveragexml";
                    break;
            }

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

                    case FileType.TrunkCoverage:
                        TrunkCoverageFilePath = dialog.FileName;
                        break;

                    case FileType.BrancheCoverage:
                        BrancheCoverageFilePath = dialog.FileName;
                        break;
                }
            }
        }

        private void ProceedMetrics()
        {
            if (!string.IsNullOrWhiteSpace(TrunkMetricsFilePath) && !string.IsNullOrWhiteSpace(BrancheMetricsFilePath))
            {
                var codeMetricsTrunkExcel = new StreamReader(TrunkMetricsFilePath);
                var codeMetricsBrancheExcel = new StreamReader(BrancheMetricsFilePath);

                var tmpTree = CodeMetricsGenerator.Generate(codeMetricsTrunkExcel, codeMetricsBrancheExcel);
                MetricsTree = new ObservableCollection<CodeMetricsLineView>(tmpTree);
            }
        }

        private void ProceedCoverage()
        {
            if (!string.IsNullOrWhiteSpace(TrunkCoverageFilePath) && !string.IsNullOrWhiteSpace(BrancheCoverageFilePath))
            {
                var codeCoverageTrunkExcel = new StreamReader(TrunkCoverageFilePath);
                var codeCoverageBrancheExcel = new StreamReader(BrancheCoverageFilePath);

                CodeCoverageGenerator.Generate(codeCoverageTrunkExcel, codeCoverageBrancheExcel);
            }
        }

        #endregion
    }
}