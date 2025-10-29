using HealthLogger.Data;
using HealthLogger.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthLogger.ViewModels
{
    public class RegistroPacienteViewModel
    {
        public Paciente Paciente { get; set; } = new Paciente();
        public ObservableCollection<string> Sexos { get; set; } =
            new ObservableCollection<string> { "M", "F" };

        public ICommand GuardarCommand => new RelayCommand(GuardarPaciente);

        private void GuardarPaciente()
        {
            PacienteRepository.AgregarPaciente(Paciente);
            System.Windows.MessageBox.Show("Paciente registrado correctamente.",
                "Registro exitoso",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);

            Paciente = new Paciente(); // Limpia el formulario
        }
    }
}
