using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace HealthLogger.Converters
{
    public class EstadoToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int estado = 0;

                if (value is bool b)
                    estado = b ? 1 : 0;
                else if (value is string s && int.TryParse(s, out int parsed))
                    estado = parsed;
                else if (value is int i)
                    estado = i;

                return estado == 0
                    ? PackIconKind.EyeOutline       // Paciente visible → Mostrar icono de “Ocultar”
                    : PackIconKind.EyeOffOutline;   // Paciente oculto → Mostrar icono de “Desocultar”
            }
            catch
            {
                return PackIconKind.Help; // fallback
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
