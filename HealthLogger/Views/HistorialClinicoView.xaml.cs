using HealthLogger.Data;
using HealthLogger.Models;
using System.Windows;
using System.Windows.Controls;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using System.IO;
using Microsoft.Win32;

namespace HealthLogger.Views
{
    public partial class HistorialClinicoView : UserControl
    {
        public Paciente Paciente { get; set; }
        public List<OpcionSexo> OpcionesSexo { get; set; }
        public List<OpcionEstadoCivil> OpcionesEstadoCivil { get; set; }

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
            OpcionesEstadoCivil = new List<OpcionEstadoCivil>
            {
                new OpcionEstadoCivil { Valor = "0", Texto = "Soltero" },
                new OpcionEstadoCivil { Valor = "1", Texto = "Casado" },
                new OpcionEstadoCivil { Valor = "2", Texto = "Viudo" },
                new OpcionEstadoCivil { Valor = "3", Texto = "Divorciado" },
                new OpcionEstadoCivil { Valor = "4", Texto = "Concubino" }
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
            originalValues[nameof(Paciente.Direccion)] = SafeStr(Paciente?.Direccion);
            originalValues[nameof(Paciente.Telefono)] = SafeStr(Paciente?.Telefono);
            originalValues[nameof(Paciente.Profesion)] = SafeStr(Paciente?.Profesion);
            originalValues[nameof(Paciente.EstadoCivil)] = SafeStr(Paciente?.EstadoCivil);
            originalValues[nameof(Paciente.Sexo)] = SafeStr(Paciente?.Sexo);
            originalValues[nameof(Paciente.FechaNacimiento)] = Paciente?.FechaNacimiento != null
                ? Paciente.FechaNacimiento.ToString("o")
                : string.Empty;
            originalValues[nameof(Paciente.ContactoEmergencia)] = SafeStr(Paciente?.ContactoEmergencia);
            originalValues[nameof(Paciente.TelefonoEmergencia)] = SafeStr(Paciente?.TelefonoEmergencia);

            originalValues[nameof(Paciente.MotivoConsulta)] = SafeStr(Paciente?.MotivoConsulta);

            originalValues[nameof(Paciente.EnfermedadActual)] = SafeStr(Paciente?.EnfermedadActual);
            originalValues[nameof(Paciente.Antecedentes)] = SafeStr(Paciente?.Antecedentes);
            originalValues[nameof(Paciente.ExamenNeurologico)] = SafeStr(Paciente?.ExamenNeurologico);
            originalValues[nameof(Paciente.ImpresionDiagnostica)] = SafeStr(Paciente?.ImpresionDiagnostica);
            originalValues[nameof(Paciente.Conducta)] = SafeStr(Paciente?.Conducta);
            originalValues[nameof(Paciente.Evolucion)] = SafeStr(Paciente?.Evolucion);
        }

        private bool HasChangesComparedToOriginal()
        {
            if (originalValues == null || originalValues.Count == 0) return false;

            if (!StringEquals(originalValues[nameof(Paciente.Nombres)], SafeStr(Paciente?.Nombres))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Apellidos)], SafeStr(Paciente?.Apellidos))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.CI)], SafeStr(Paciente?.CI))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Direccion)], SafeStr(Paciente?.Direccion))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Telefono)], SafeStr(Paciente?.Telefono))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Sexo)], SafeStr(Paciente?.Sexo))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Profesion)], SafeStr(Paciente?.Profesion))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.EstadoCivil)], SafeStr(Paciente?.EstadoCivil))) return true;

            var origFecha = originalValues[nameof(Paciente.FechaNacimiento)];
            var currentFecha = Paciente?.FechaNacimiento != null ? Paciente.FechaNacimiento.ToString("o") : string.Empty;
            if (!StringEquals(origFecha, currentFecha)) return true;

            if (!StringEquals(originalValues[nameof(Paciente.ContactoEmergencia)], SafeStr(Paciente?.ContactoEmergencia))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.TelefonoEmergencia)], SafeStr(Paciente?.TelefonoEmergencia))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.MotivoConsulta)], SafeStr(Paciente?.MotivoConsulta))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.EnfermedadActual)], SafeStr(Paciente?.EnfermedadActual))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Antecedentes)], SafeStr(Paciente?.Antecedentes))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.ExamenNeurologico)], SafeStr(Paciente?.ExamenNeurologico))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.ImpresionDiagnostica)], SafeStr(Paciente?.ImpresionDiagnostica))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Conducta)], SafeStr(Paciente?.Conducta))) return true;
            if (!StringEquals(originalValues[nameof(Paciente.Evolucion)], SafeStr(Paciente?.Evolucion))) return true;
            return false;
        }

        private static string SafeStr(string s) => s ?? string.Empty;
        private static bool StringEquals(string a, string b) =>
            string.Equals((a ?? string.Empty).Trim(), (b ?? string.Empty).Trim(), StringComparison.Ordinal);

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

        private void AnyFieldChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void GenerarPdf_Click(object sender, RoutedEventArgs e)
        {
            if (Paciente == null)
            {
                MessageBox.Show("No se puede generar el PDF porque no se cargaron los datos del paciente.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                CrearPdfHistorialClinico();
            }
        }
        private void CrearPdfHistorialClinico()
        {
            var doc = new Document();
            doc.Info.Title = "Historial Clínico de Paciente";

            var section = doc.AddSection();
            section.PageSetup.PageFormat = PageFormat.Letter;
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2);
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);

            // Título
            var titulo = section.AddParagraph("Historial Clínico");
            titulo.Format.Font.Size = 20;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceAfter = Unit.FromCentimeter(0.5);
            titulo.Format.Alignment = ParagraphAlignment.Center;

            // --- DATOS PERSONALES ---
            section.AddParagraph("Datos de Filiación").Format.Font.Bold = true;

            section.AddParagraph($"Nombre: {Paciente.Nombres} {Paciente.Apellidos}");
            section.AddParagraph($"C.I.: {Paciente.CI}");
            section.AddParagraph($"Profesión: {Paciente.Profesion}");
            section.AddParagraph($"Dirección: {Paciente.Direccion}");
            section.AddParagraph($"Teléfono: {Paciente.Telefono}");
            section.AddParagraph($"Contacto de Emergencia: {Paciente.ContactoEmergencia} - {Paciente.TelefonoEmergencia}");
            var sexoTexto = ObtenerTextoSexo(Paciente.Sexo);
            var estadoCivilTexto = ObtenerTextoEstadoCivil(Paciente.EstadoCivil);

            section.AddParagraph($"Género: {sexoTexto}");
            section.AddParagraph($"Estado Civil: {estadoCivilTexto}");

            //section.AddParagraph($"Fecha de Nacimiento: {Paciente.FechaNacimiento:dd/MM/yyyy}");
            section.AddParagraph($"Edad: {Paciente.Edad}");

            section.AddParagraph("");

            // --- CAMPOS LARGOS (tipo documento) ---
            void AddBlock(string tituloBloque, string contenido)
            {
                var t = section.AddParagraph(tituloBloque);
                t.Format.Font.Bold = true;
                t.Format.SpaceBefore = Unit.FromCentimeter(0.5);

                var p = section.AddParagraph(string.IsNullOrWhiteSpace(contenido) ? "—" : contenido);
                p.Format.SpaceAfter = Unit.FromCentimeter(0.5);
            }

            AddBlock("Motivo de Consulta", Paciente.MotivoConsulta);
            AddBlock("Enfermedad Actual", Paciente.EnfermedadActual);
            AddBlock("Antecedentes - Alergias", Paciente.Antecedentes);
            AddBlock("Examen Neurológico", Paciente.ExamenNeurologico);
            AddBlock("Impresión Diagnóstica", Paciente.ImpresionDiagnostica);
            AddBlock("Conducta", Paciente.Conducta);
            AddBlock("Evolución", Paciente.Evolucion);

            // Guardar archivo
            var saveDialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = $"{Paciente.Nombres}_{Paciente.Apellidos}_HistorialClinico.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var renderer = new PdfDocumentRenderer(true)
                {
                    Document = doc
                };
                renderer.RenderDocument();
                renderer.PdfDocument.Save(saveDialog.FileName);

                MessageBox.Show("PDF generado correctamente.", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private string ObtenerTextoSexo(string valor)
        {
            return OpcionesSexo.FirstOrDefault(o => o.Valor == valor)?.Texto ?? "—";
        }

        private string ObtenerTextoEstadoCivil(string valor)
        {
            return OpcionesEstadoCivil.FirstOrDefault(o => o.Valor == valor)?.Texto ?? "—";
        }


    }
}
