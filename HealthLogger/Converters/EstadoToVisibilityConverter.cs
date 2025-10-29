using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HealthLogger.Converters
{
    public class EstadoToVisibilityConverter : IValueConverter
    {
        // ConverterParameter: "VisibleWhenActive" o "VisibleWhenHidden"
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int estado)
            {
                if (parameter?.ToString() == "VisibleWhenActive")
                    return estado == 0 ? Visibility.Visible : Visibility.Collapsed; // 0 = activo → mostrar botón Ocultar
                else if (parameter?.ToString() == "VisibleWhenHidden")
                    return estado == 1 ? Visibility.Visible : Visibility.Collapsed; // 1 = oculto → mostrar botón Desocultar
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
