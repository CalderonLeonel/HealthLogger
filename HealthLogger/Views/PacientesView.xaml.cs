using HealthLogger.Converters;
using HealthLogger.Data;
using HealthLogger.Models;
using System.Collections.Generic;
using System.Linq;
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

            // Columna ID
            var idCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "ID",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)), // gris medio transparente
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                Binding = new Binding("Id"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)),
                MinWidth = 40,
                MaxWidth = 50
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

            // Columna Nombre completo
            var nombreCompletoCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "Nombre",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding() { Converter = new NombreCompletoConverter() },
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 100,
                MaxWidth = 300
            };
            nombreCompletoCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis),
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(nombreCompletoCol);

            // Columna CI
            var ciCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "CI",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding("CI"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 85,
                MaxWidth = 100
            };
            ciCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(ciCol);

            // Columna Sexo con íconos
            var sexoCol = new DataGridTemplateColumn
            {
                Header = new TextBlock
                {
                    Text = "Sexo",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 65,
                MaxWidth = 65
            };

            // Plantilla de celda: ícono azul si "M", rosa si "F"
            var factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            factory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);

            var icon = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            icon.SetBinding(MaterialDesignThemes.Wpf.PackIcon.KindProperty, new Binding("Sexo")
            {
                Converter = new SexoToIconConverter()
            });
            icon.SetBinding(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, new Binding("Sexo")
            {
                Converter = new SexoToColorConverter()
            });
            icon.SetValue(FrameworkElement.WidthProperty, 22.0);
            icon.SetValue(FrameworkElement.HeightProperty, 22.0);

            factory.AppendChild(icon);

            // Asignar plantilla
            sexoCol.CellTemplate = new DataTemplate { VisualTree = factory };

            // Agregar columna
            DataPacientes.Columns.Add(sexoCol);

            // Columna Teléfono
            var telefonoCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "Teléfono",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding("Telefono"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 100,
                MaxWidth = 180
            };
            telefonoCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap),
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(telefonoCol);

            // Columna Edad
            var edadCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "Edad",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding("Edad"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                MinWidth = 65,
                MaxWidth = 65
            };
            edadCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(edadCol);

            // Columna Creado
            var creadoCol = new DataGridTextColumn
            {
                Header = new TextBlock
                {
                    Text = "Creado el",
                    FontWeight = FontWeights.Bold,  // Header en negrita
                    Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)), // gris medio transparente
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Binding = new Binding("CreatedAt") { StringFormat = "dd/MM/yyyy HH:mm" },
                Foreground = new SolidColorBrush(Color.FromArgb(170, 136, 136, 136)),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
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
            modificadoCol.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap),
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.FontWeightProperty, FontWeights.Bold)
                }
            };
            DataPacientes.Columns.Add(modificadoCol);

            // Columna de botones (opciones)
            var plantilla = new DataTemplate();
            var spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            spFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Left);

            // ===== Botón Ver paciente =====
            var verBtn = new FrameworkElementFactory(typeof(Button));
            verBtn.SetValue(Button.MarginProperty, new Thickness(2));
            verBtn.SetValue(Button.ToolTipProperty, "Ver paciente");
            verBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(VerPaciente_Click));
            verBtn.SetBinding(Button.TagProperty, new Binding("Id"));
            verBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            verBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            verBtn.SetValue(FrameworkElement.WidthProperty, 36.0);
            verBtn.SetValue(FrameworkElement.HeightProperty, 36.0);
            verBtn.SetValue(Button.PaddingProperty, new Thickness(0));
            verBtn.SetValue(Button.BorderThicknessProperty, new Thickness(0));
            verBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            verBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            verBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignFlatButton") as Style);
            verBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignIconButton") as Style);

            var iconoVer = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconoVer.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.EyeOutline);
            iconoVer.SetValue(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, Brushes.DodgerBlue);

            iconoVer.SetValue(FrameworkElement.WidthProperty, 20.0);
            iconoVer.SetValue(FrameworkElement.HeightProperty, 20.0);
            verBtn.AppendChild(iconoVer);

            spFactory.AppendChild(verBtn);

            // ===== Botón Ocultar paciente =====
            var ocultarBtn = new FrameworkElementFactory(typeof(Button));
            ocultarBtn.SetValue(Button.MarginProperty, new Thickness(2));
            ocultarBtn.SetValue(Button.ToolTipProperty, "Ocultar paciente");
            ocultarBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(OcultarPaciente_Click));
            ocultarBtn.SetBinding(Button.TagProperty, new Binding("Id"));
            ocultarBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            ocultarBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            verBtn.SetValue(FrameworkElement.WidthProperty, 36.0);
            verBtn.SetValue(FrameworkElement.HeightProperty, 36.0);
            verBtn.SetValue(Button.PaddingProperty, new Thickness(0));
            verBtn.SetValue(Button.BorderThicknessProperty, new Thickness(0));
            verBtn.SetValue(Button.BackgroundProperty, Brushes.Transparent);
            verBtn.SetValue(Button.BorderBrushProperty, Brushes.Transparent);
            verBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignFlatButton") as Style);
            verBtn.SetValue(Button.StyleProperty, Application.Current.FindResource("MaterialDesignIconButton") as Style);

            var iconoOcultar = new FrameworkElementFactory(typeof(MaterialDesignThemes.Wpf.PackIcon));
            iconoOcultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline);
            iconoOcultar.SetValue(MaterialDesignThemes.Wpf.PackIcon.ForegroundProperty, Brushes.DodgerBlue);
            iconoOcultar.SetValue(FrameworkElement.WidthProperty, 20.0);
            iconoOcultar.SetValue(FrameworkElement.HeightProperty, 20.0);
            ocultarBtn.AppendChild(iconoOcultar);

            spFactory.AppendChild(ocultarBtn);

            // ===== Añadir columna al DataGrid =====
            plantilla.VisualTree = spFactory;

            DataPacientes.Columns.Add(new DataGridTemplateColumn
            {
                Header = new TextBlock
                {
                    Text = "Opciones",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                MinWidth = 100,
                CellTemplate = plantilla
            });
        }


        private void CargarPacientes()
        {
            todosPacientes = PacienteRepository.ObtenerPacientes();
            DataPacientes.ItemsSource = null; // asegura que esté limpia
            DataPacientes.ItemsSource = todosPacientes;
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

        private void OcultarPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int id))
            {
                var res = MessageBox.Show("¿Ocultar este paciente? Esto no eliminará sus datos.",
                                          "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    PacienteRepository.OcultarPaciente(id);
                    CargarPacientes();
                }
            }
        }
    }
}