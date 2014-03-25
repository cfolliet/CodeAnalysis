namespace CodeAnalysis.Views
{
    using System.Windows;
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
    }
}