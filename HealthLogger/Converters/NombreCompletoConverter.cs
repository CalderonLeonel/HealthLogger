using System.Globalization;
using System.Windows.Data;
using HealthLogger.Models;

namespace HealthLogger.Converters
{
    public class NombreCompletoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Paciente p)
                return $"{p.Nombres} {p.Apellidos}";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}