using K_NECT.Data;
using K_NECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace K_NECT.Views.Estudiante
{
    public partial class MisAsignaturasView : Window
    {
        private K_NECT.Models.Estudiante _estudianteActual;

        public MisAsignaturasView()
        {
            InitializeComponent();
        }

        public MisAsignaturasView(K_NECT.Models.Estudiante estudiante) : this()
        {
            _estudianteActual = estudiante;
            CargarDatos();
        }

        // ===== CARGAR DATOS DEL ESTUDIANTE =====
        private void CargarDatos()
        {
            if (_estudianteActual == null) return;

            // Mostrar info del estudiante
            txtEstudiante.Text = $"Estudiante: {_estudianteActual.NombreCompleto}";
            txtCarreraSemestre.Text = $"{_estudianteActual.Carrera} - Semestre {_estudianteActual.Semestre}";

            // Cargar asignaturas
            CargarAsignaturas();
        }

        // ===== CARGAR ASIGNATURAS =====
        private void CargarAsignaturas()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var asignaturas = context.ESTUDIANTE_ASIGNATURA
                        .Where(ea => ea.IdEstudiante == _estudianteActual.IdEstudiante)
                        .Select(ea => new
                        {
                            ea.IdEstudianteAsignatura,
                            ea.Asignatura.IdAsignatura,
                            ea.Asignatura.CodigoAsignatura,
                            ea.Asignatura.NombreAsignatura,
                            ea.Asignatura.Creditos,
                            ea.Estado,
                            ea.CicloAcademico
                        })
                        .ToList();

                    if (asignaturas.Any())
                    {
                        txtCantidadAsignaturas.Text = asignaturas.Count.ToString();

                        List<decimal> promedios = new List<decimal>();

                        foreach (var asignatura in asignaturas)
                        {
                            // Obtener calificaciones de esta asignatura
                            var calificaciones = context.EVALUACION_BASE
                                .Where(eb => eb.IdEstudianteAsignatura == asignatura.IdEstudianteAsignatura)
                                .ToList();

                            decimal promedio = 0;
                            if (calificaciones.Any())
                            {
                                promedio = calificaciones.Average(c => c.NotaObtenida);
                                promedios.Add(promedio);
                            }

                            // Crear tarjeta de asignatura
                            CrearTarjetaAsignatura(
                                asignatura.CodigoAsignatura,
                                asignatura.NombreAsignatura,
                                asignatura.Creditos,
                                asignatura.Estado,
                                promedio,
                                calificaciones.Count
                            );
                        }

                        // Calcular promedio general
                        if (promedios.Any())
                        {
                            txtPromedioGeneral.Text = promedios.Average().ToString("F2");
                        }
                        else
                        {
                            txtPromedioGeneral.Text = "N/A";
                        }

                        pnlAsignaturas.Visibility = Visibility.Visible;
                        pnlSinAsignaturas.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        pnlAsignaturas.Visibility = Visibility.Collapsed;
                        pnlSinAsignaturas.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas:\n{ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // ===== CREAR TARJETA DE ASIGNATURA =====
        private void CrearTarjetaAsignatura(string codigo, string nombre, int creditos,
                                           string estado, decimal promedio, int cantidadNotas)
        {
            // Border principal
            var border = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D9D9")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(25),
                Margin = new Thickness(0, 0, 0, 15),
                Cursor = Cursors.Hand
            };

            // Efecto hover
            border.MouseEnter += (s, e) =>
            {
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60A5FA"));
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
            };

            border.MouseLeave += (s, e) =>
            {
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D9D9"));
            };

            // Grid interno
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // ===== COLUMNA 1: INFORMACIÓN DE LA ASIGNATURA =====
            var stackInfo = new StackPanel();

            // Código y nombre
            var stackHeader = new StackPanel { Orientation = Orientation.Horizontal };

            var txtCodigo = new TextBlock
            {
                Text = codigo,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A")),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 15, 0)
            };

            var txtNombre = new TextBlock
            {
                Text = nombre,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                FontSize = 16,
                FontWeight = FontWeights.SemiBold
            };

            stackHeader.Children.Add(txtCodigo);
            stackHeader.Children.Add(txtNombre);
            stackInfo.Children.Add(stackHeader);

            // Información adicional
            var txtInfo = new TextBlock
            {
                Text = $"📖 Créditos: {creditos} | 📊 Evaluaciones: {cantidadNotas}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")),
                FontSize = 13,
                Margin = new Thickness(0, 8, 0, 0)
            };

            stackInfo.Children.Add(txtInfo);

            Grid.SetColumn(stackInfo, 0);
            grid.Children.Add(stackInfo);

            // ===== COLUMNA 2: PROMEDIO =====
            var borderPromedio = new Border
            {
                Background = promedio >= 6
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626")),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(15, 0, 15, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            var stackPromedio = new StackPanel();

            var txtPromedioLabel = new TextBlock
            {
                Text = "PROMEDIO",
                Foreground = Brushes.White,
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var txtPromedioValor = new TextBlock
            {
                Text = cantidadNotas > 0 ? promedio.ToString("F2") : "N/A",
                Foreground = Brushes.White,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            stackPromedio.Children.Add(txtPromedioLabel);
            stackPromedio.Children.Add(txtPromedioValor);
            borderPromedio.Child = stackPromedio;

            Grid.SetColumn(borderPromedio, 1);
            grid.Children.Add(borderPromedio);

            // ===== COLUMNA 3: ESTADO =====
            var borderEstado = new Border
            {
                Background = estado == "CURSANDO"
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
                CornerRadius = new CornerRadius(15),
                VerticalAlignment = VerticalAlignment.Center
            };

            var txtEstado = new TextBlock
            {
                Text = estado,
                Foreground = Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };

            borderEstado.Child = txtEstado;

            Grid.SetColumn(borderEstado, 2);
            grid.Children.Add(borderEstado);

            // Agregar grid al border
            border.Child = grid;

            // Agregar al panel
            pnlAsignaturas.Children.Add(border);

            // Evento click para ver detalles
            border.MouseDown += (s, e) =>
            {
                MostrarDetallesAsignatura(codigo, nombre, promedio, cantidadNotas);
            };
        }

        // ===== MOSTRAR DETALLES DE ASIGNATURA =====
        private void MostrarDetallesAsignatura(string codigo, string nombre, decimal promedio, int cantidadNotas)
        {
            string mensaje = $"📚 DETALLES DE LA ASIGNATURA\n\n" +
                           $"Código: {codigo}\n" +
                           $"Nombre: {nombre}\n" +
                           $"Promedio actual: {(cantidadNotas > 0 ? promedio.ToString("F2") : "Sin calificaciones")}\n" +
                           $"Evaluaciones registradas: {cantidadNotas}\n\n" +
                           $"💡 Próximamente podrás ver el detalle completo de tus calificaciones.";

            MessageBox.Show(mensaje,
                          "Detalles de Asignatura",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        // ===== EVENTOS DE VENTANA =====
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}