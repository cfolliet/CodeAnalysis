namespace CodeAnalysis.ViewModels
{
    internal class ViewModel : BaseViewModel
    {
        public ViewModel()
        {
            Text = "lola";
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }
    }
}