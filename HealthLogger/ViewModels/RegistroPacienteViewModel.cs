using HealthLogger.Data;
using HealthLogger.Models;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace HealthLogger.ViewModels
{
    public class RegistroPacienteViewModel
    {
        public Paciente Paciente { get; set; } = new Paciente();
        public List<OpcionSexo> OpcionesSexo { get; set; }

        public ICommand GuardarCommand => new RelayCommand(GuardarPaciente);

        public RegistroPacienteViewModel()
        {
            Paciente.FechaNacimiento = new DateTime(2000, 1, 1);

            OpcionesSexo = new List<OpcionSexo>
            {
                new OpcionSexo { Valor = "M", Texto = "Hombre" },
                new OpcionSexo { Valor = "F", Texto = "Mujer" }
            };
        }

        private void GuardarPaciente()
        {
            try
            {
                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(Paciente.Nombres) ||
                    string.IsNullOrWhiteSpace(Paciente.Apellidos) ||
                    string.IsNullOrWhiteSpace(Paciente.Sexo))
                {
                    MessageBox.Show("Los campos Nombres, Apellidos, Fecha de Nacimiento y Sexo son obligatorios.",
                                    "Campos requeridos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Reemplazar null por cadena vacía para evitar errores en la BD
                Paciente.CI ??= "";
                Paciente.Telefono ??= "";
                Paciente.ContactoEmergencia ??= "";
                Paciente.TelefonoEmergencia ??= "";
                Paciente.Antecedentes ??= "";
                Paciente.Alergias ??= "";
                Paciente.Observaciones ??= "";

                PacienteRepository.AgregarPaciente(Paciente);

                MessageBox.Show("Paciente registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "HealthLogger",
                    "error.log"
                );

                File.AppendAllText(logPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {ex.Message}\n{ex.StackTrace}\n\n");
                MessageBox.Show(
                    $"Ocurrió un error al guardar el paciente:\n\n{ex.Message}\n\nDetalles:\n{ex.StackTrace}",
                    "Error inesperado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

    }
}
