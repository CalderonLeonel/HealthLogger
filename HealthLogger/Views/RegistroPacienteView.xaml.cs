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
    }
}
