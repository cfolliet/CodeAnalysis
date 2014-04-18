﻿namespace CodeAnalysis.ViewModels
{
    using System;
    using System.Collections.Generic;
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
            BrowseCodeMetricsTrunkFileCommand = new RelayCommand(param => BrowseFiles(FileType.TrunkMetrics));
            BrowseCodeMetricsBrancheFileCommand = new RelayCommand(param => BrowseFiles(FileType.BrancheMetrics));
            ProceedCodeMetricsCommand = new RelayCommand(param => ProceedCodeMetrics());

            BrowseCodeCoverageTrunkFileCommand = new RelayCommand(param => BrowseFiles(FileType.TrunkCoverage));
            BrowseCodeCoverageBrancheFileCommand = new RelayCommand(param => BrowseFiles(FileType.BrancheCoverage));
            ProceedCodeCoverageCommand = new RelayCommand(param => ProceedCodeCoverage());


            string[] args = Environment.GetCommandLineArgs();

            if (args.Length >= 3)
            {
                CodeMetricsTrunkFilePath = args[1];
                CodeMetricsBrancheFilePath = args[2];
                ProceedCodeMetrics();
            }
        }

        #endregion

        #region Commands

        public RelayCommand BrowseCodeMetricsTrunkFileCommand { get; set; }
        public RelayCommand BrowseCodeMetricsBrancheFileCommand { get; set; }
        public RelayCommand ProceedCodeMetricsCommand { get; set; }

        public RelayCommand BrowseCodeCoverageTrunkFileCommand { get; set; }
        public RelayCommand BrowseCodeCoverageBrancheFileCommand { get; set; }
        public RelayCommand ProceedCodeCoverageCommand { get; set; }

        #endregion

        #region Properties

        private string codeMetricsTrunkFilePath;

        public string CodeMetricsTrunkFilePath
        {
            get { return codeMetricsTrunkFilePath; }
            set { codeMetricsTrunkFilePath = value; OnPropertyChanged("CodeMetricsTrunkFilePath"); }
        }

        private string codeMetricsBrancheFilePath;

        public string CodeMetricsBrancheFilePath
        {
            get { return codeMetricsBrancheFilePath; }
            set { codeMetricsBrancheFilePath = value; OnPropertyChanged("CodeMetricsBrancheFilePath"); }
        }

        private ObservableCollection<CodeMetricsLineView> codeMetricsTree;

        public ObservableCollection<CodeMetricsLineView> CodeMetricsTree
        {
            get { return codeMetricsTree; }
            set { codeMetricsTree = value; OnPropertyChanged("CodeMetricsTree"); }
        }

        private string codeCoverageTrunkFilePath;

        public string CodeCoverageTrunkFilePath
        {
            get { return codeCoverageTrunkFilePath; }
            set { codeCoverageTrunkFilePath = value; OnPropertyChanged("CodeCoverageTrunkFilePath"); }
        }

        private string codeCoverageBrancheFilePath;

        public string CodeCoverageBrancheFilePath
        {
            get { return codeCoverageBrancheFilePath; }
            set { codeCoverageBrancheFilePath = value; OnPropertyChanged("CodeCoverageBrancheFilePath"); }
        }

        private ObservableCollection<CodeCoverageLineView> codeCoverageTree;

        public ObservableCollection<CodeCoverageLineView> CodeCoverageTree
        {
            get { return codeCoverageTree; }
            set { codeCoverageTree = value; OnPropertyChanged("CodeCoverageTree"); }
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
                    dialog.Filter = "Code Metrics Files|*.xlsx;*.xml";
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
                        CodeMetricsTrunkFilePath = dialog.FileName;
                        break;

                    case FileType.BrancheMetrics:
                        CodeMetricsBrancheFilePath = dialog.FileName;
                        break;

                    case FileType.TrunkCoverage:
                        CodeCoverageTrunkFilePath = dialog.FileName;
                        break;

                    case FileType.BrancheCoverage:
                        CodeCoverageBrancheFilePath = dialog.FileName;
                        break;
                }
            }
        }

        private void ProceedCodeMetrics()
        {
            if (!string.IsNullOrWhiteSpace(CodeMetricsTrunkFilePath) && !string.IsNullOrWhiteSpace(CodeMetricsBrancheFilePath))
            {
                const string EXTENSION_XLSX = ".xlsx";
                const string EXTENSION_XML = ".xml";

                var codeMetricsTrunkFileExtension = Path.GetExtension(codeMetricsTrunkFilePath);
                var codeMetricsBrancheFileExtension = Path.GetExtension(codeMetricsBrancheFilePath);

                IEnumerable<CodeMetricsLineView> tmpTree;

                if (codeMetricsTrunkFileExtension == EXTENSION_XLSX && codeMetricsBrancheFileExtension == EXTENSION_XLSX)
                {
                    tmpTree = CodeMetricsExcelGenerator.Generate(CodeMetricsTrunkFilePath, CodeMetricsBrancheFilePath);
                }
                else if (codeMetricsTrunkFileExtension == EXTENSION_XML && codeMetricsBrancheFileExtension == EXTENSION_XML)
                {
                    tmpTree = CodeMetricsXmlGenerator.Generate(CodeMetricsTrunkFilePath, CodeMetricsBrancheFilePath);
                }
                else
                {
                    throw new InvalidOperationException("codeMetricsBrancheFile extension invalid");
                }

                CodeMetricsTree = new ObservableCollection<CodeMetricsLineView>(tmpTree);
            }
        }

        private void ProceedCodeCoverage()
        {
            if (!string.IsNullOrWhiteSpace(CodeCoverageTrunkFilePath) && !string.IsNullOrWhiteSpace(CodeCoverageBrancheFilePath))
            {
                var codeCoverageTrunkFile = new StreamReader(CodeCoverageTrunkFilePath);
                var codeCoverageBrancheFile = new StreamReader(CodeCoverageBrancheFilePath);

                var tmpTree = CodeCoverageGenerator.Generate(codeCoverageTrunkFile, codeCoverageBrancheFile);
                CodeCoverageTree = new ObservableCollection<CodeCoverageLineView>(tmpTree);
            }
        }


        #endregion
    }
}