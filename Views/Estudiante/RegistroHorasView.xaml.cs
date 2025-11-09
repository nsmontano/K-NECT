using K_NECT.Data;
using K_NECT.Models;
using K_NECT.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace K_NECT.Views.Estudiante
{
    public partial class RegistroHorasView : Window
    {
        private K_NECT.Models.Estudiante _estudianteActual;
        private List<RegistroHorasDisplay> _registros;

        public RegistroHorasView()
        {
            InitializeComponent();
        }

        public RegistroHorasView(K_NECT.Models.Estudiante estudiante) : this()
        {
            _estudianteActual = estudiante;
            Loaded += RegistroHorasView_Loaded;
        }

        private void RegistroHorasView_Loaded(object sender, RoutedEventArgs e)
        {
            txtEstudianteInfo.Text = _estudianteActual.NombreCompleto;
            dpFecha.SelectedDate = DateTime.Now;
            CargarAsignaturas();
            CargarRegistros();
        }

        // ===== CARGAR ASIGNATURAS =====
        private void CargarAsignaturas()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var asignaturas = context.ESTUDIANTE_ASIGNATURA
                        .Where(ea => ea.IdEstudiante == _estudianteActual.IdEstudiante && ea.Estado == "CURSANDO")
                        .Select(ea => new
                        {
                            ea.IdEstudianteAsignatura,
                            ea.Asignatura.IdAsignatura,
                            ea.Asignatura.CodigoAsignatura,
                            ea.Asignatura.NombreAsignatura
                        })
                        .ToList();

                    if (asignaturas.Any())
                    {
                        cmbAsignaturas.ItemsSource = asignaturas;
                        cmbAsignaturas.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No tienes asignaturas cursando actualmente.",
                                      "Sin Asignaturas",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
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

        // ===== CARGAR REGISTROS =====
        private void CargarRegistros()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var registros = context.REGISTRO_HORAS_ESTUDIO
                        .Where(r => r.EstudianteAsignatura.IdEstudiante == _estudianteActual.IdEstudiante)
                        .OrderByDescending(r => r.Fecha)
                        .ToList();

                    _registros = registros.Select(r => new RegistroHorasDisplay
                    {
                        IdRegistro = r.IdRegistro,
                        FechaFormateada = r.Fecha.ToString("dd/MM/yyyy"),
                        NombreAsignatura = r.EstudianteAsignatura?.Asignatura?.NombreAsignatura ?? "(Sin asignatura)",
                        HorasEstudio = r.HorasEstudio.ToString("F2") + " hrs",
                        TemaEstudiado = r.TemaEstudiado,
                        Observaciones = r.Observaciones ?? ""
                    }).ToList();


                    if (_registros.Any())
                    {
                        dgRegistros.ItemsSource = _registros;
                        dgRegistros.Visibility = Visibility.Visible;
                        pnlSinRegistros.Visibility = Visibility.Collapsed;

                        // Calcular estadísticas
                        ActualizarEstadisticas(registros);
                    }
                    else
                    {
                        dgRegistros.Visibility = Visibility.Collapsed;
                        pnlSinRegistros.Visibility = Visibility.Visible;

                        txtTotalRegistros.Text = "0";
                        txtTotalHoras.Text = "0.0";
                        txtPromedioHoras.Text = "0.0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar registros:\n{ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // ===== ACTUALIZAR ESTADÍSTICAS =====
        private void ActualizarEstadisticas(List<RegistroHorasEstudio> registros)
        {
            txtTotalRegistros.Text = registros.Count.ToString();

            decimal totalHoras = registros.Sum(r => r.HorasEstudio);
            txtTotalHoras.Text = totalHoras.ToString("F1");

            // Calcular promedio por día de estudio
            var diasUnicos = registros.Select(r => r.Fecha.Date).Distinct().Count();
            decimal promedio = diasUnicos > 0 ? totalHoras / diasUnicos : 0;
            txtPromedioHoras.Text = promedio.ToString("F1");
        }

        // ===== GUARDAR REGISTRO =====
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos
            if (cmbAsignaturas.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona una asignatura",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            if (!dpFecha.SelectedDate.HasValue)
            {
                MessageBox.Show("Por favor selecciona una fecha",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtHoras.Text, out int horas) || horas < 0 || horas > 23)
            {
                MessageBox.Show("Las horas deben ser un número entre 0 y 23",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                txtHoras.Focus();
                return;
            }

            if (!int.TryParse(txtMinutos.Text, out int minutos) || minutos < 0 || minutos > 59)
            {
                MessageBox.Show("Los minutos deben ser un número entre 0 y 59",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                txtMinutos.Focus();
                return;
            }

            if (horas == 0 && minutos == 0)
            {
                MessageBox.Show("Debes ingresar al menos 1 minuto de estudio",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTema.Text))
            {
                MessageBox.Show("Por favor ingresa el tema estudiado",
                              "Validación",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                txtTema.Focus();
                return;
            }

            try
            {
                dynamic asignaturaSeleccionada = cmbAsignaturas.SelectedItem;
                int idEstudianteAsignatura = asignaturaSeleccionada.IdEstudianteAsignatura;

                // Convertir a decimal (horas + minutos/60)
                decimal horasDecimal = horas + (minutos / 60.0m);

                using (var context = new ApplicationDbContext())
                {
                    var nuevoRegistro = new RegistroHorasEstudio
                    {
                        IdEstudianteAsignatura = idEstudianteAsignatura,
                        Fecha = dpFecha.SelectedDate.Value,
                        HorasEstudio = horasDecimal,
                        TemaEstudiado = txtTema.Text.Trim(),
                        Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim()
                    };

                    context.REGISTRO_HORAS_ESTUDIO.Add(nuevoRegistro);
                    context.SaveChanges();

                    MessageBox.Show("✅ Registro guardado exitosamente",
                                  "Éxito",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    // Limpiar y recargar
                    LimpiarFormulario();
                    CargarRegistros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar registro:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }
        // ===== LIMPIAR FORMULARIO =====
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            if (cmbAsignaturas.Items.Count > 0)
                cmbAsignaturas.SelectedIndex = 0;

            dpFecha.SelectedDate = DateTime.Now;
            txtHoras.Text = "0";
            txtMinutos.Text = "0";
            txtTema.Clear();
            txtObservaciones.Clear();
            txtTema.Focus();
        }

        // ===== ELIMINAR REGISTRO =====
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null || button.Tag == null) return;

            int idRegistro = (int)button.Tag;

            var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?",
                                        "Confirmar Eliminación",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var registro = context.REGISTRO_HORAS_ESTUDIO.Find(idRegistro);
                        if (registro != null)
                        {
                            context.REGISTRO_HORAS_ESTUDIO.Remove(registro);
                            context.SaveChanges();

                            MessageBox.Show("Registro eliminado exitosamente",
                                          "Éxito",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);

                            CargarRegistros();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar registro:\n{ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
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

    // ===== CLASE AUXILIAR PARA MOSTRAR REGISTROS =====
    public class RegistroHorasDisplay
    {
        public int IdRegistro { get; set; }
        public string FechaFormateada { get; set; }
        public string NombreAsignatura { get; set; }
        public string HorasEstudio { get; set; }
        public string TemaEstudiado { get; set; }
        public string Observaciones { get; set; }
    }
}