using System.Windows;
using System.Windows.Controls;
using HealthLogger.Views;

namespace HealthLogger
{
    public class OpcionSexo
    {
        public string Valor { get; set; }   // "M" o "F"
        public string Texto { get; set; }   // "Hombre" o "Mujer"
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Mostrar vista inicial al abrir
            MainFrame.Content = new PacientesView();
        }
        private void NavigateTo(UserControl view)
        {
            MainFrame.Opacity = 0;
            MainFrame.Content = view;

            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            MainFrame.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void RegistrarPaciente_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new RegistroPacienteView());
        }

        private void BuscarPacientes_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new PacientesView());
        }



        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}