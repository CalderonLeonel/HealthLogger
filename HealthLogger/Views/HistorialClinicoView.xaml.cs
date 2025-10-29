using HealthLogger.Data;
using HealthLogger.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HealthLogger.Views
{
    public class OpcionSexo
    {
        public string Valor { get; set; }   // "M" o "F"
        public string Texto { get; set; }   // "Hombre" o "Mujer"
    }

    public partial class HistorialClinicoView : UserControl
    {
        public Paciente Paciente { get; set; }
        public List<OpcionSexo> OpcionesSexo { get; set; }

        private bool hayCambios = false;
        private Dictionary<string, string> originalValues = new Dictionary<string, string>();

        public HistorialClinicoView(int pacienteId)
        {
            InitializeComponent();

            // Inicializar lista de opciones
            OpcionesSexo = new List<OpcionSexo>
            {
                new OpcionSexo { Valor = "M", Texto = "Hombre" },
                new OpcionSexo { Valor = "F", Texto = "Mujer" }
            };

            // Obtener paciente desde la base de datos
            Paciente = PacienteRepository.ObtenerPacientePorId(pacienteId);

            // Asignar el contexto de datos a esta vista (para acceder a Paciente y OpcionesSexo)
            DataContext = this;

            Loaded += HistorialClinicoView_Loaded;
        }

        private void HistorialClinicoView_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureOriginalValues();
            hayCambios = false;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (Paciente != null)
            {
                PacienteRepository.ActualizarPaciente(Paciente);
                MessageBox.Show("Cambios guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CaptureOriginalValues();
                hayCambios = false;
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            if (hayCambios)
            {
                var res = MessageBox.Show("Hay cambios no guardados. ¿Deseas salir sin guardar?",
                                          "Confirmar salida",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);
                if (res == MessageBoxResult.No) return;
            }

            var main = Application.Current.MainWindow as MainWindow;
            if (main != null)
                main.MainFrame.Content = new PacientesView();
        }

        private void AnyFieldChanged(object sender, RoutedEventArgs e)
        {
            hayCambios = HasChangesComparedToOriginal();
        }

        private void FechaNacimiento_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            hayCambios = HasChangesComparedToOriginal();
        }

        // 🔽 Métodos para snapshot de cambios (igual que antes)
        private void CaptureOriginalValues()
        {
            originalValues.Clear();

            originalValues[nameof(Paciente.Nombres)] = SafeStr(Paciente?.Nombres);
            originalValues[nameof(Paciente.Apellidos)] = SafeStr(Paciente?.Apellidos);
            originalValues[nameof(Paciente.CI)] = SafeStr(Paciente?.CI);
            originalValues[nameof(Paciente.Telefono)] = SafeStr(Paciente?.Telefono);
            originalValues[nameof(Paciente.Sexo)] = SafeStr(Paciente?.Sexo);
            originalValues[nameof(Paciente.FechaNacimiento)] = Paciente?.FechaNacimiento != null
                ? Paciente.FechaNacimiento.ToString("o")
                : string.Empty;
            originalValues[nameof(Paciente.Antecedentes)] = SafeStr(Paciente?.Antecedentes);
            originalValues[nameof(Paciente.Alergias)] = SafeStr(Paciente?.Alergias);
            originalValues[nameof(Paciente.Observaciones)] = SafeStr(Paciente?.Observaciones);
        }

        private bool HasChangesComparedToOriginal()
        {
            if (originalValues == null || originalValues.Count == 0) return false;

            if (!StringEquals(originalValues[nameof(Paciente.Nombres)], SafeStr(Paciente?.Nombres))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Apellidos)], SafeStr(Paciente?.Apellidos))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.CI)], SafeStr(Paciente?.CI))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Telefono)], SafeStr(Paciente?.Telefono))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Sexo)], SafeStr(Paciente?.Sexo))) return true;

            var origFecha = originalValues[nameof(Paciente.FechaNacimiento)];
            var currentFecha = Paciente?.FechaNacimiento != null ? Paciente.FechaNacimiento.ToString("o") : string.Empty;
            if (!StringEquals(origFecha, currentFecha)) return true;

            if (!StringEquals(originalValues[nameof(Paciente.Antecedentes)], SafeStr(Paciente?.Antecedentes))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Alergias)], SafeStr(Paciente?.Alergias))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Observaciones)], SafeStr(Paciente?.Observaciones))) return true;

            return false;
        }

        private static string SafeStr(string s) => s ?? string.Empty;
        private static bool StringEquals(string a, string b) =>
            string.Equals((a ?? string.Empty).Trim(), (b ?? string.Empty).Trim(), StringComparison.Ordinal);
    }
}
