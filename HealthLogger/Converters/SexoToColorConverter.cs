using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HealthLogger.Converters
{
    public class SexoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexo = value?.ToString()?.ToUpper() ?? "";
            return new SolidColorBrush(sexo == "M"
                ? Color.FromRgb(33, 150, 243)  // Azul (Material Blue 500)
                : Color.FromRgb(233, 30, 99)); // Rosa (Material Pink 500)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
