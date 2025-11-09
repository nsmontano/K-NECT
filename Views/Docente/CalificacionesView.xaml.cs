using K_NECT.Data;
using K_NECT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace K_NECT.Views.Docente
{
    public partial class CalificacionesView : Window
    {
        private K_NECT.Models.Docente _docenteActual;
        private List<CalificacionEstudiante> _calificaciones;
        private bool _cambiosPendientes = false;

        public CalificacionesView()
        {
            InitializeComponent();
        }

        public CalificacionesView(K_NECT.Models.Docente docente) : this()
        {
            _docenteActual = docente;
            CargarAsignaturasDelDocente();
        }

        // ===== CARGAR ASIGNATURAS =====
        private void CargarAsignaturasDelDocente()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var asignaturas = context.DOCENTE_ASIGNATURA
                        .Where(da => da.IdDocente == _docenteActual.IdDocente)
                        .Select(da => new
                        {
                            da.Asignatura.IdAsignatura,
                            da.Asignatura.CodigoAsignatura,
                            da.Asignatura.NombreAsignatura
                        })
                        .ToList();

                    cmbAsignaturas.ItemsSource = asignaturas;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas:\n{ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== CAMBIO DE ASIGNATURA =====
        private void CmbAsignaturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAsignaturas.SelectedItem != null && cmbTipoEvaluacion.SelectedItem != null)
            {
                CargarEvaluaciones();
            }
        }

        // ===== CAMBIO DE TIPO DE EVALUACIÓN =====
        private void CmbTipoEvaluacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAsignaturas.SelectedItem != null && cmbTipoEvaluacion.SelectedItem != null)
            {
                CargarEvaluaciones();
            }
        }

        // ===== CARGAR EVALUACIONES DISPONIBLES =====
        private void CargarEvaluaciones()
        {
            if (cmbAsignaturas.SelectedItem == null || cmbTipoEvaluacion.SelectedItem == null)
                return;

            try
            {
                dynamic asignaturaSeleccionada = cmbAsignaturas.SelectedItem;
                int idAsignatura = asignaturaSeleccionada.IdAsignatura;

                string tipoSeleccionado = ((ComboBoxItem)cmbTipoEvaluacion.SelectedItem).Content.ToString();

                using (var context = new ApplicationDbContext())
                {
                    if (tipoSeleccionado.Contains("Parciales"))
                    {
                        var parciales = context.PARCIAL
                            .Where(p => p.IdAsignatura == idAsignatura)
                            .ToList();

                        cmbEvaluaciones.ItemsSource = parciales;
                        cmbEvaluaciones.DisplayMemberPath = "NombreParcial";
                    }
                    else if (tipoSeleccionado.Contains("Tareas"))
                    {
                        var tareas = context.TAREA
                            .Where(t => t.IdAsignatura == idAsignatura)
                            .ToList();

                        cmbEvaluaciones.ItemsSource = tareas;
                        cmbEvaluaciones.DisplayMemberPath = "NombreTarea";
                    }
                    else if (tipoSeleccionado.Contains("Proyectos"))
                    {
                        var proyectos = context.PROYECTO
                            .Where(p => p.IdAsignatura == idAsignatura)
                            .ToList();

                        cmbEvaluaciones.ItemsSource = proyectos;
                        cmbEvaluaciones.DisplayMemberPath = "NombreProyecto";
                    }
                }

                pnlSelectorEvaluacion.Visibility = Visibility.Visible;
                btnNuevaEvaluacion.Visibility = Visibility.Visible;

                if (cmbEvaluaciones.Items.Count > 0)
                {
                    cmbEvaluaciones.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar evaluaciones:\n{ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== CAMBIO DE EVALUACIÓN ESPECÍFICA =====
        private void CmbEvaluaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEvaluaciones.SelectedItem != null)
            {
                CargarCalificaciones();
            }
        }

        // ===== CARGAR CALIFICACIONES =====
        private void CargarCalificaciones()
        {
            try
            {
                dynamic asignaturaSeleccionada = cmbAsignaturas.SelectedItem;
                int idAsignatura = asignaturaSeleccionada.IdAsignatura;

                string tipoSeleccionado = ((ComboBoxItem)cmbTipoEvaluacion.SelectedItem).Content.ToString();
                dynamic evaluacionSeleccionada = cmbEvaluaciones.SelectedItem;

                using (var context = new ApplicationDbContext())
                {
                    var estudiantes = context.ESTUDIANTE_ASIGNATURA
                        .Where(ea => ea.IdAsignatura == idAsignatura)
                        .ToList();

                    _calificaciones = new List<CalificacionEstudiante>();

                    foreach (var ea in estudiantes)
                    {
                        var estudiante = context.ESTUDIANTE.Find(ea.IdEstudiante);

                        var calificacion = new CalificacionEstudiante
                        {
                            IdEstudianteAsignatura = ea.IdEstudianteAsignatura,
                            CodigoEstudiante = estudiante.CodigoEstudiante,
                            NombreCompleto = estudiante.NombreCompleto,
                            Nota = 0,
                            Comentario = "",
                            Estado = "Sin calificar"
                        };

                        // Buscar si ya existe calificación
                        EvaluacionBase evaluacionExistente = null;

                        if (tipoSeleccionado.Contains("Parciales"))
                        {
                            var parcial = evaluacionSeleccionada as Parcial;
                            evaluacionExistente = context.EVALUACION_BASE
                                .FirstOrDefault(eb => eb.IdEstudianteAsignatura == ea.IdEstudianteAsignatura
                                               && eb.TipoEvaluacion == "PARCIAL");
                        }

                        if (evaluacionExistente != null)
                        {
                            calificacion.IdEvaluacion = evaluacionExistente.IdEvaluacionBase;
                            calificacion.Nota = evaluacionExistente.NotaObtenida;
                            calificacion.Comentario = evaluacionExistente.Comentario ?? "";
                            calificacion.Estado = "Calificado";
                        }

                        _calificaciones.Add(calificacion);
                    }

                    dgCalificaciones.ItemsSource = _calificaciones;
                    dgCalificaciones.Visibility = Visibility.Visible;
                    pnlSinDatos.Visibility = Visibility.Collapsed;

                    txtInfoEvaluacion.Text = $"📝 {asignaturaSeleccionada.CodigoAsignatura} - ";
                    btnGuardar.IsEnabled = true;
                    _cambiosPendientes = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar calificaciones:\n{ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== EDICIÓN DE CELDAS =====
        private void DgCalificaciones_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var calificacion = e.Row.Item as CalificacionEstudiante;
                if (calificacion != null)
                {
                    // Validar nota
                    if (e.Column.Header.ToString() == "Nota (0-10)")
                    {
                        var textBox = e.EditingElement as TextBox;
                        if (textBox != null)
                        {
                            if (decimal.TryParse(textBox.Text, out decimal nota))
                            {
                                if (nota < 0 || nota > 10)
                                {
                                    MessageBox.Show("La nota debe estar entre 0 y 10", "Validación",
                                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    _cambiosPendientes = true;
                    calificacion.Estado = "Modificado";
                }
            }
        }

        // ===== GUARDAR CALIFICACIONES =====
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_calificaciones == null || !_calificaciones.Any())
            {
                MessageBox.Show("No hay calificaciones para guardar", "Información",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                dynamic evaluacionSeleccionada = cmbEvaluaciones.SelectedItem;
                string tipoSeleccionado = ((ComboBoxItem)cmbTipoEvaluacion.SelectedItem).Content.ToString();

                using (var context = new ApplicationDbContext())
                {
                    int guardadas = 0;

                    foreach (var calificacion in _calificaciones.Where(c => c.Nota > 0))
                    {
                        // Buscar si ya existe la evaluación
                        var evaluacionExistente = context.EVALUACION_BASE
                            .FirstOrDefault(eb => eb.IdEvaluacionBase == calificacion.IdEvaluacion);

                        if (evaluacionExistente != null)
                        {
                            // Actualizar
                            evaluacionExistente.NotaObtenida = calificacion.Nota;
                            evaluacionExistente.Comentario = calificacion.Comentario;
                            evaluacionExistente.FechaModificacion = DateTime.Now;
                        }
                        else
                        {
                            // Crear nueva evaluación base
                            var nuevaEvaluacion = new EvaluacionBase
                            {
                                IdEstudianteAsignatura = calificacion.IdEstudianteAsignatura,
                                TipoEvaluacion = tipoSeleccionado.Contains("Parciales") ? "PARCIAL" :
                                               tipoSeleccionado.Contains("Tareas") ? "TAREA" : "PROYECTO",
                                NotaObtenida = calificacion.Nota,
                                PesoPorcentual = ObtenerPesoPorcentual(evaluacionSeleccionada),
                                Comentario = calificacion.Comentario,
                                FechaRegistro = DateTime.Now
                            };

                            context.EVALUACION_BASE.Add(nuevaEvaluacion);
                            context.SaveChanges();

                            // Si es parcial, crear registro en EVALUACION_PARCIAL
                            if (tipoSeleccionado.Contains("Parciales"))
                            {
                                var parcial = evaluacionSeleccionada as Parcial;
                                var evaluacionParcial = new EvaluacionParcial
                                {
                                    IdEvaluacionBase = nuevaEvaluacion.IdEvaluacionBase,
                                    IdParcial = parcial.IdParcial,
                                    EsRecuperacion = false
                                };

                                context.EVALUACION_PARCIAL.Add(evaluacionParcial);
                            }
                        }

                        guardadas++;
                    }

                    context.SaveChanges();

                    txtEstadoGuardado.Text = $"✅ {guardadas} calificaciones guardadas exitosamente - {DateTime.Now:HH:mm:ss}";
                    _cambiosPendientes = false;

                    MessageBox.Show($"✅ Se guardaron {guardadas} calificaciones exitosamente",
                                  "Guardado Exitoso",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    // Recargar calificaciones
                    CargarCalificaciones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar calificaciones:\n{ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // ===== OBTENER PESO PORCENTUAL =====
        private decimal ObtenerPesoPorcentual(dynamic evaluacion)
        {
            try
            {
                if (evaluacion is Parcial parcial)
                    return parcial.ValorPorcentual;
                else if (evaluacion is Tarea tarea)
                    return tarea.ValorPorcentual;
                else if (evaluacion is Proyecto proyecto)
                    return 40.00M; // Valor por defecto para proyectos

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        // ===== NUEVA EVALUACIÓN =====
        private void BtnNuevaEvaluacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad de crear nueva evaluación próximamente...",
                          "Información",
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
            if (_cambiosPendientes)
            {
                var result = MessageBox.Show("Hay cambios sin guardar. ¿Desea salir sin guardar?",
                                           "Confirmar",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;
            }

            this.Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            BtnVolver_Click(sender, null);
        }
    }
    // ===== CLASE AUXILIAR PARA CALIFICACIONES =====
    public class CalificacionEstudiante : INotifyPropertyChanged
    {
        public int IdEvaluacion { get; set; }
        public int IdEstudianteAsignatura { get; set; }
        public string CodigoEstudiante { get; set; }
        public string NombreCompleto { get; set; }
        private decimal _nota;
        public decimal Nota
        {
            get => _nota;
            set
            {
                if (_nota != value)
                {
                    _nota = value;
                    OnPropertyChanged(nameof(Nota));
                }
            }
        }

        private string _comentario;
        public string Comentario
        {
            get => _comentario;
            set
            {
                if (_comentario != value)
                {
                    _comentario = value;
                    OnPropertyChanged(nameof(Comentario));
                }
            }
        }

        private string _estado;
        public string Estado
        {
            get => _estado;
            set
            {
                if (_estado != value)
                {
                    _estado = value;
                    OnPropertyChanged(nameof(Estado));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}