using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace HealthLogger.Converters
{
    public class SexoToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexo = value?.ToString()?.ToUpper() ?? "";
            return sexo == "M" ? PackIconKind.GenderMale : PackIconKind.GenderFemale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
