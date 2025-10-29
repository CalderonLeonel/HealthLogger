using System;
using System.Globalization;
using System.Windows.Data;
using System.Diagnostics;
namespace HealthLogger.Converters
{
    public class EstadoToToolTipConverter : IValueConverter
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

                // Si está activo (1): debe poder ocultarse.
                return estado == 0 ? "Ocultar paciente" : "Desocultar paciente";
            }
            catch
            {
                return "Estado desconocido";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
