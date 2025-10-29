using HealthLogger.Data;
using HealthLogger.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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

            //Paciente = new Paciente { FechaNacimiento = new DateTime(2000, 1, 1) };
        }
    }
}
