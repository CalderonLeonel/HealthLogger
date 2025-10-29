using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthLogger.ViewModels;

namespace HealthLogger.Views
{
    /// <summary>
    /// Lógica de interacción para RegistroPacienteView.xaml
    /// </summary>
    public partial class RegistroPacienteView : UserControl
    {
        public RegistroPacienteView()
        {
            InitializeComponent();
            DataContext = new RegistroPacienteViewModel();
        }
    }
}
