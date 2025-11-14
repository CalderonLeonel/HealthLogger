using HealthLogger.Converters;
using HealthLogger.Data;
using HealthLogger.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HealthLogger.Views
{
    public partial class PacientesView : UserControl
    {
        private List<Paciente> todosPacientes;

        public PacientesView()
        {
            InitializeComponent();
            ConfigurarColumnas();
            CargarPacientes();
        }

        private void ConfigurarColumnas()
        {
            DataPacientes.Columns.Clear();
            // DataTrigger para Estado = 1
            var trigger = new DataTrigger
            {
                Binding = new Binding("Estado"),
                Value = 0 // paciente oculto
            };
            trigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(Color.FromArgb(170, 136, 136, 136))));

            // --- Columna ID ---
            var idCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "ID",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)),
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                Binding = new Binding("Id"),
                Width = 50,
                Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)),
                MinWidth = 40,
                MaxWidth = 60
            };
            idCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
        {
            new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis),
            new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
            new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
            new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
        }
            };
            DataPacientes.Columns.Add(idCol);

            // --- Columna Nombre completo ---
            var nombreCol = new DataGridTextColumn
            {
                Header = new TextBlock { Text = "Nombre", FontWeight = FontWeights.Bold },
                Binding = new Binding() { Converter = new NombreCompletoConverter() },
                Width = 300,//new DataGridLength(2, DataGridLengthUnitType.Star), //Calcula el width segun el espacio que haya
                MinWidth = 100,
                MaxWidth = 350
            };
            nombreCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis),
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            // Estilo con trigger para cambiar color si Estado = 0 (oculto)
            var nombreStyle = new Style(typeof(TextBlock));
            nombreStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left));
            nombreStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            nombreStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            nombreStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black)); // color por defecto

            nombreStyle.Triggers.Add(trigger);

            nombreCol.ElementStyle = nombreStyle;
            DataPacientes.Columns.Add(nombreCol);

            // --- Columna CI ---
            var ciCol = new DataGridTextColumn
            {
                Header = new TextBlock { Text = "CI", FontWeight = FontWeights.Bold },
                Binding = new Binding("CI"),
                Width = 100, //new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 85,
                MaxWidth = 130
            };
            // Estilo con trigger para cambiar color si Estado = 0 (oculto)
            var ciStyle = new Style(typeof(TextBlock));
            ciStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left));
            ciStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            ciStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            ciStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black)); // color por defecto

            ciStyle.Triggers.Add(trigger);

            ciCol.ElementStyle = ciStyle;
            DataPacientes.Columns.Add(ciCol);

            // --- Columna Sexo (icono) ---
            var sexoCol = new DataGridTemplateColumn
            {
                Header = new TextBlock { Text = "Sexo", FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Left },
                Width = 65, //new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 10,
                MaxWidth = 65
            };
            var sexoFactory = new FrameworkElementFactory(typeof(StackPanel));
            sexoFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            sexoFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            sexoFactory.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);

            var iconSexo = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconSexo.SetBinding(MaterialDesignThemes.Wpf.PackIcon.KindProperty, new Binding("Sexo") { Converter = new SexoToIconConverter() });
            iconSexo.SetBinding(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, new Binding("Sexo") { Converter = new SexoToColorConverter() });
            iconSexo.SetValue(FrameworkElement.WidthProperty, 22.0);
            iconSexo.SetValue(FrameworkElement.HeightProperty, 22.0);

            sexoFactory.AppendChild(iconSexo);
            sexoCol.CellTemplate = new DataTemplate { VisualTree = sexoFactory };
            DataPacientes.Columns.Add(sexoCol);

            // --- Columna Teléfono ---
            var telefonoCol = new DataGridTextColumn
            {
                Header = new TextBlock { Text = "Teléfono", FontWeight = FontWeights.Bold },
                Binding = new Binding("Telefono"),
                Width = 120,//new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 100,
                MaxWidth = 180
            };

            // Estilo con trigger para cambiar color si Estado = 0 (oculto)
            var telefonoStyle = new Style(typeof(TextBlock));
            telefonoStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            telefonoStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left));
            telefonoStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            telefonoStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            telefonoStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black)); // color por defecto

            telefonoStyle.Triggers.Add(trigger);

            telefonoCol.ElementStyle = telefonoStyle;

            DataPacientes.Columns.Add(telefonoCol);


            // --- Columna Edad ---
            var edadCol = new DataGridTextColumn
            {
                Header = new TextBlock { Text = "Edad", FontWeight = FontWeights.Bold },
                Binding = new Binding("Edad"),
                Width = 65,
                MinWidth = 65,
                MaxWidth = 65
            };
            var edadStyle = new Style(typeof(TextBlock));
            edadStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            edadStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left));
            edadStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            edadStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            edadStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black)); // color por defecto

            edadStyle.Triggers.Add(trigger);

            edadCol.ElementStyle = edadStyle;
            DataPacientes.Columns.Add(edadCol);

            // --- Columna Creado el ---
            var creadoCol = new DataGridTextColumn
            {
                Header = new TextBlock { Text = "Creado el", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)) },
                Binding = new Binding("CreatedAt") { StringFormat = "dd/MM/yyyy HH:mm" },
                Width = 120,
                Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)),
                MinWidth = 100,
                MaxWidth = 120
            };
            creadoCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap),
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(creadoCol);
            // Columna Última mod.
            var modificadoCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "Última mod.",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding("UpdatedAt") { StringFormat = "dd/MM/yyyy HH:mm" },
                Width = 120,
                MinWidth = 100,
                MaxWidth = 120
            };
            //
            // Estilo con trigger para cambiar color si Estado = 1 (oculto)
            var modificadoStyle = new Style(typeof(TextBlock));
            modificadoStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            modificadoStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left));
            modificadoStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            modificadoStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            modificadoStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Black)); // color por defecto

            modificadoStyle.Triggers.Add(trigger);

            modificadoCol.ElementStyle = modificadoStyle;
            //
            DataPacientes.Columns.Add(modificadoCol);



            // --- Columna Opciones (botones Seleccionar y Ocultar/Desocultar) ---
            var plantilla = new DataTemplate();
            var spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            spFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Left);

            // --- Botón Ver ---
            var seleccionarBtn = new FrameworkElementFactory(typeof(Button));
            seleccionarBtn.SetValue(Button.MarginProperty, new Thickness(2));
            seleccionarBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(VerPaciente_Click));
            seleccionarBtn.SetBinding(Button.TagProperty, new Binding("Id"));
            seleccionarBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            seleccionarBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            seleccionarBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignIconButton") as Style);

            var iconVer = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconVer.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.FileText);
            iconVer.SetValue(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, Brushes.DodgerBlue);
            iconVer.SetValue(FrameworkElement.WidthProperty, 20.0);
            iconVer.SetValue(FrameworkElement.HeightProperty, 20.0);
            seleccionarBtn.AppendChild(iconVer);
            spFactory.AppendChild(seleccionarBtn);

            // --- Botón Desocultar ---
            var desocultarBtn = new FrameworkElementFactory(typeof(Button));
            desocultarBtn.SetValue(Button.MarginProperty, new Thickness(2));
            desocultarBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(DesocultarPaciente_Click));
            desocultarBtn.SetBinding(Button.TagProperty, new Binding("Id"));
            desocultarBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            desocultarBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            desocultarBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignIconButton") as Style);

            // Icono del botón Ocultar
            var iconDesocultar = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconDesocultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.EyeOutline);
            iconDesocultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, Brushes.DodgerBlue);
            iconDesocultar.SetValue(FrameworkElement.WidthProperty, 20.0);
            iconDesocultar.SetValue(FrameworkElement.HeightProperty, 20.0);
            desocultarBtn.AppendChild(iconDesocultar);

            // Visibilidad según Estado = 0 (activo → visible)
            var visibilidadOcultarBinding = new Binding("Estado")
            {
                Converter = new EstadoToVisibilityConverter(),
                ConverterParameter = "VisibleWhenActive"
            };
            desocultarBtn.SetBinding(Button.VisibilityProperty, visibilidadOcultarBinding);

            // --- Botón Ocultar ---
            var ocultarBtn = new FrameworkElementFactory(typeof(Button));
            ocultarBtn.SetValue(Button.MarginProperty, new Thickness(2));
            ocultarBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OcultarPaciente_Click));
            ocultarBtn.SetBinding(Button.TagProperty, new Binding("Id"));
            ocultarBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            ocultarBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            ocultarBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignIconButton") as Style);

            // Icono del botón Ocultar
            var iconOcultar = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconOcultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline);
            iconOcultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, Brushes.DodgerBlue);
            iconOcultar.SetValue(FrameworkElement.WidthProperty, 20.0);
            iconOcultar.SetValue(FrameworkElement.HeightProperty, 20.0);
            ocultarBtn.AppendChild(iconOcultar);

            // Visibilidad según Estado = 1 (oculto → visible)
            var visibilidadDesocultarBinding = new Binding("Estado")
            {
                Converter = new EstadoToVisibilityConverter(),
                ConverterParameter = "VisibleWhenHidden"
            };
            ocultarBtn.SetBinding(Button.VisibilityProperty, visibilidadDesocultarBinding);

            // Agregar ambos botones al StackPanel
            spFactory.AppendChild(ocultarBtn);
            spFactory.AppendChild(desocultarBtn);


            plantilla.VisualTree = spFactory;

            DataPacientes.Columns.Add(new DataGridTemplateColumn
            {
                Header = new TextBlock { Text = "Opciones", FontWeight = FontWeights.Bold },
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                MinWidth = 100,
                CellTemplate = plantilla
            });
        }

        private void CargarPacientes()
        {
            bool incluirOcultos = ChkMostrarOcultos.IsChecked ?? false;
            todosPacientes = PacienteRepository.ObtenerPacientes(incluirOcultos);
            AplicarFiltros();
        }
        private void AplicarFiltros()
        {
            string filtro = TxtBuscar.Text.ToLower();
            var filtrados = todosPacientes
                .Where(p => p.Nombres.ToLower().Contains(filtro) ||
                            p.Apellidos.ToLower().Contains(filtro))
                .ToList();

            DataPacientes.ItemsSource = filtrados;

            // Pintar filas de pacientes ocultos
            foreach (var item in DataPacientes.Items)
            {
                if (item is Paciente p && p.Estado == 0)
                {
                    var row = DataPacientes.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                        row.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220)); // plomo suave
                }
            }
        }
        private void ChkMostrarOcultos_Checked(object sender, RoutedEventArgs e)
        {
            CargarPacientes();
        }
        private void DesocultarPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int id))
            {
                var paciente = todosPacientes.FirstOrDefault(p => p.Id == id);
                if (paciente == null) return;

                var res = MessageBox.Show("¿Desocultar este paciente?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    PacienteRepository.DesocultarPaciente(id);
                    paciente.Estado = 1; // cambia, dispara OnPropertyChanged
                }
            }
        }

        private void OcultarPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int id))
            {
                var paciente = todosPacientes.FirstOrDefault(p => p.Id == id);
                if (paciente == null) return;

                var res = MessageBox.Show("¿Ocultar este paciente?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    PacienteRepository.OcultarPaciente(id);
                    paciente.Estado = 0; // cambia, dispara OnPropertyChanged
                    CargarPacientes();
                }
            }
        }
        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = TxtBuscar.Text.ToLower();
            var filtrados = todosPacientes
                .Where(p => p.Nombres.ToLower().Contains(filtro) ||
                            p.Apellidos.ToLower().Contains(filtro))
                .ToList();

            DataPacientes.ItemsSource = filtrados;
        }

        private void VerPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int id))
            {
                var main = Application.Current.MainWindow as MainWindow;
                if (main != null)
                {
                    main.MainFrame.Content = new HistorialClinicoView(id);
                }
            }
        }
    }
}