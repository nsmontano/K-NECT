using K_NECT.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace K_NECT.Views.Docente
{
    public partial class MenuDocenteView : Window
    {
        public K_NECT.Models.Docente DocenteActual { get; set; }

        public MenuDocenteView()
        {
            InitializeComponent();
        }

        public MenuDocenteView(K_NECT.Models.Docente docente) : this()
        {
            DocenteActual = docente;
            CargarDatosDocente();
        }

        // ===== CARGAR DATOS DEL DOCENTE =====
        // ===== CARGAR DATOS DEL DOCENTE =====
        private void CargarDatosDocente()
        {
            if (DocenteActual != null)
            {
                txtNombreDocente.Text = DocenteActual.NombreCompleto;
                txtEspecialidad.Text = DocenteActual.Especialidad;
                txtBienvenida.Text = $"¡Bienvenido, {DocenteActual.Nombres}! 👨‍🏫";
                txtBienvenidaDetalle.Text = $"Hoy es {DateTime.Now:dddd, dd 'de' MMMM 'de' yyyy}";

                // ===== DATOS DE PRUEBA O ESTADÍSTICAS =====
                // En una app real, esto vendrá de la BD
                DocenteActual.CantidadAsignaturas = 5;
                DocenteActual.CantidadEstudiantes = 120;
                DocenteActual.EvaluacionesPendientes = 15;

                // Mostrar estadísticas
                txtEstadisticasDocente.Text =
                    $"📚 {DocenteActual.CantidadAsignaturas} asignaturas | " +
                    $"👨‍🎓 {DocenteActual.CantidadEstudiantes} estudiantes | " +
                    $"📝 {DocenteActual.EvaluacionesPendientes} evaluaciones pendientes";
            }
        }



        // ===== PERMITIR ARRASTRAR LA VENTANA =====
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // ===== EVENTOS DEL MENÚ LATERAL =====
        private void MenuItem_Inicio_Click(object sender, RoutedEventArgs e)
        {
            // Ya estamos en inicio
        }

        private void MenuItem_Asignaturas_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Navegando a Mis Asignaturas...");
        }

        private void MenuItem_Estudiantes_Click(object sender, RoutedEventArgs e)
        {
            var gestionView = new GestionEstudiantesView(DocenteActual);
            gestionView.ShowDialog();
        }

        private void MenuItem_Calificaciones_Click(object sender, RoutedEventArgs e)
        {
            var calificacionesView = new CalificacionesView(DocenteActual);
            calificacionesView.ShowDialog();
        }

        private void MenuItem_Asistencias_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Navegando a Control de Asistencia...");
        }

        private void MenuItem_Reportes_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Navegando a Reportes...");
        }

        private void MenuItem_Perfil_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Navegando a Mi Perfil...");
        }

        // ===== EVENTOS DE LAS TARJETAS =====
        private void Card_Asignaturas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Asignaturas_Click(sender, null);
        }

        private void Card_Estudiantes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Estudiantes_Click(sender, null);
        }

        private void Card_Calificaciones_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Calificaciones_Click(sender, null);
        }

        private void Card_Asistencias_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Asistencias_Click(sender, null);
        }

        private void Card_Reportes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Reportes_Click(sender, null);
        }

        private void Card_Perfil_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Perfil_Click(sender, null);
        }

        // ===== BOTONES DEL HEADER =====
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Desea volver a la pantalla de inicio?", "Confirmar",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void BtnNotificaciones_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Mostrando notificaciones...");
        }

        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Abriendo configuración...");
        }

        // ===== CERRAR SESIÓN =====
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea cerrar sesión?", "Cerrar Sesión",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        // ===== UTILIDADES =====
        private void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "K-NECT Docente",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}