namespace CodeAnalysis.Views
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using CodeAnalysis.Models;
    using CodeAnalysis.ViewModels;

    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        public View()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        private void OpenFileDiff(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            const string TortoisemergeExe = "C:/Program Files/TortoiseSVN/bin/TortoiseMerge.exe";

            var grid = sender as Grid;
            if (grid == null) return;

            var lineView = grid.DataContext as CodeMetricsLineView;
            if(lineView == null) return;

            if (lineView.Type != null && Environment.GetCommandLineArgs().Length >= 5)
            {
                var devRepoPath = Environment.GetCommandLineArgs()[3];
                var sprintRepoPath = Environment.GetCommandLineArgs()[4];

                var devFile = Directory.GetFiles(devRepoPath, lineView.Type + ".cs", SearchOption.AllDirectories).FirstOrDefault();
                var sprintFile = Directory.GetFiles(sprintRepoPath, lineView.Type + ".cs", SearchOption.AllDirectories).FirstOrDefault();

                Process.Start(TortoisemergeExe, "/base:\"" + devFile + "\" /mine:\"" + sprintFile + "\"");
            }
        }
    }
}