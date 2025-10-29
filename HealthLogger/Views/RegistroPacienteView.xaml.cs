using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using HealthLogger.ViewModels;

namespace HealthLogger.Views
{
    public partial class RegistroPacienteView : UserControl
    {
        public RegistroPacienteView()
        {
            InitializeComponent();
            DataContext = new RegistroPacienteViewModel();
        }

        private void SoloLetras(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$");
        }

        private void SoloNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9+]+$");
        }
        private void AutoGrowTextBox(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                const double lineHeight = 16; // tamaño de letra aproximado
                int visibleLines = 4; // líneas visibles iniciales (equivalentes al MinHeight)
                double minHeight = textBox.MinHeight > 0 ? textBox.MinHeight : visibleLines * lineHeight + 10;

                // Calcula el número estimado de líneas del texto
                int lineCount = textBox.LineCount > 0 ? textBox.LineCount : textBox.Text.Split('\n').Length;

                // Calcula la altura deseada (una línea extra por si acaso)
                double desiredHeight = Math.Max(minHeight, (lineCount + 3) * lineHeight + 30);

                // Limita el crecimiento máximo (opcional)
                double maxHeight = 600; // evita que se alargue demasiado
                desiredHeight = Math.Min(desiredHeight, maxHeight);

                textBox.Height = desiredHeight;
            }
        }
    }
}
