namespace CodeAnalysis.Views
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class BrushConverter : FrameworkElement, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int i;
                Int32.TryParse(value.ToString(), out i);

                if (i < 0)
                {
                    return new SolidColorBrush(Colors.Red);
                }

                if (i > 0)
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}