using K_NECT.Data;
using K_NECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace K_NECT.Views.Docente
{
    public partial class GestionEstudiantesView : Window
    {
        private K_NECT.Models.Docente _docenteActual;
        private List<EstudianteConEstado> _todosLosEstudiantes;

        public GestionEstudiantesView()
        {
            InitializeComponent();
        }

        public GestionEstudiantesView(K_NECT.Models.Docente docente) : this()
        {
            _docenteActual = docente;
            CargarAsignaturasDelDocente();
        }

        // ===== CARGAR ASIGNATURAS DEL DOCENTE =====
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
                            da.Asignatura.NombreAsignatura,
                            da.Grupo,
                            da.CicloAcademico
                        })
                        .ToList();

                    if (asignaturas.Any())
                    {
                        cmbAsignaturas.ItemsSource = asignaturas;
                        cmbAsignaturas.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No tienes asignaturas asignadas en este momento.",
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

        // ===== CARGAR ESTUDIANTES DE LA ASIGNATURA =====
        private void CmbAsignaturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAsignaturas.SelectedItem == null) return;

            try
            {
                dynamic asignaturaSeleccionada = cmbAsignaturas.SelectedItem;
                int idAsignatura = asignaturaSeleccionada.IdAsignatura;

                using (var context = new ApplicationDbContext())
                {
                    _todosLosEstudiantes = context.ESTUDIANTE_ASIGNATURA
                        .Where(ea => ea.IdAsignatura == idAsignatura)
                        .Select(ea => new EstudianteConEstado
                        {
                            CodigoEstudiante = ea.Estudiante.CodigoEstudiante,
                            Nombres = ea.Estudiante.Nombres,
                            Apellidos = ea.Estudiante.Apellidos,
                            CorreoEstudiante = ea.Estudiante.CorreoEstudiante,
                            Carrera = ea.Estudiante.Carrera,
                            Semestre = ea.Estudiante.Semestre ?? 0,
                            EstadoInscripcion = ea.Estado
                        })
                        .ToList();

                    ActualizarVistaEstudiantes(_todosLosEstudiantes);

                    // Actualizar información
                    txtInfoAsignatura.Text = $"📚 {asignaturaSeleccionada.CodigoAsignatura} - {asignaturaSeleccionada.NombreAsignatura}";
                    txtCantidadEstudiantes.Text = $"| Total: {_todosLosEstudiantes.Count} estudiantes";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar estudiantes:\n{ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // ===== ACTUALIZAR VISTA DE ESTUDIANTES =====
        private void ActualizarVistaEstudiantes(List<EstudianteConEstado> estudiantes)
        {
            if (estudiantes == null || !estudiantes.Any())
            {
                dgEstudiantes.Visibility = Visibility.Collapsed;
                pnlSinEstudiantes.Visibility = Visibility.Visible;
            }
            else
            {
                dgEstudiantes.ItemsSource = estudiantes;
                dgEstudiantes.Visibility = Visibility.Visible;
                pnlSinEstudiantes.Visibility = Visibility.Collapsed;
            }
        }

        // ===== BÚSQUEDA DE ESTUDIANTES =====
        private void TxtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Buscar por nombre o código...")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar por nombre o código...";
                txtBuscar.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_todosLosEstudiantes == null || txtBuscar.Text == "Buscar por nombre o código...")
                return;

            string busqueda = txtBuscar.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(busqueda))
            {
                ActualizarVistaEstudiantes(_todosLosEstudiantes);
                return;
            }

            var estudiantesFiltrados = _todosLosEstudiantes
                .Where(a => a.CodigoEstudiante.ToLower().Contains(busqueda) ||
                           a.Nombres.ToLower().Contains(busqueda) ||
                           a.Apellidos.ToLower().Contains(busqueda) ||
                           a.CorreoEstudiante.ToLower().Contains(busqueda))
                .ToList();

            ActualizarVistaEstudiantes(estudiantesFiltrados);
            txtCantidadEstudiantes.Text = $"| Mostrando: {estudiantesFiltrados.Count} de {_todosLosEstudiantes.Count}";
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

    // ===== CLASE AUXILIAR PARA MOSTRAR ESTUDIANTES =====
    public class EstudianteConEstado
    {
        public string CodigoEstudiante { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoEstudiante { get; set; }
        public string Carrera { get; set; }
        public int Semestre { get; set; }
        public string EstadoInscripcion { get; set; }
    }
}