using K_NECT.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace K_NECT.Views.Estudiante
{
    public partial class MenuEstudianteView : Window
    {
        public K_NECT.Models.Estudiante EstudianteActual { get; set; }
        

        public MenuEstudianteView()
        {
            InitializeComponent();
        }

        public MenuEstudianteView(K_NECT.Models.Estudiante estudiante) : this()
        {
            EstudianteActual = estudiante;
            CargarDatosEstudiante();
        }

        // ===== CARGAR DATOS DEL ESTUDIANTE =====
        private void CargarDatosEstudiante()
        {
            if (EstudianteActual != null)
            {
                txtNombreEstudiante.Text = EstudianteActual.NombreCompleto;
                txtCodigoEstudiante.Text = $"Código: {EstudianteActual.CodigoEstudiante}";
                txtBienvenida.Text = $"¡Bienvenido de vuelta, {EstudianteActual.Nombres}! 🎓";

                // Actualizar estadísticas (por ahora datos de ejemplo)
                // ===== MOSTRAR ESTADÍSTICAS DEL ESTUDIANTE =====
                if (EstudianteActual.FechaNacimiento.HasValue)
                {
                    int edad = DateTime.Today.Year - EstudianteActual.FechaNacimiento.Value.Year;
                    if (EstudianteActual.FechaNacimiento.Value.Date > DateTime.Today.AddYears(-edad)) edad--;

                    txtEstadisticas.Text =
                        $"🎓 Carrera: {EstudianteActual.Carrera}\n" +
                        $"📘 Semestre: {EstudianteActual.Semestre}\n" +
                        $"🎂 Edad: {edad} años\n" +
                        $"📞 Teléfono: {(string.IsNullOrWhiteSpace(EstudianteActual.Telefono) ? "No registrado" : EstudianteActual.Telefono)}\n" +
                        $"📧 Correo: {EstudianteActual.CorreoEstudiante}";
                }
                else
                {
                    txtEstadisticas.Text =
                        $"🎓 Carrera: {EstudianteActual.Carrera}\n" +
                        $"📘 Semestre: {EstudianteActual.Semestre}\n" +
                        $"📞 Teléfono: {(string.IsNullOrWhiteSpace(EstudianteActual.Telefono) ? "No registrado" : EstudianteActual.Telefono)}\n" +
                        $"📧 Correo: {EstudianteActual.CorreoEstudiante}";
                }

            }
        }

        // ===== PERMITIR ARRASTRAR LA VENTANA =====
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // ===== EVENTOS DE NAVEGACIÓN =====
        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            // Ya estamos en inicio
        }

        private void BtnAsignaturas_Click(object sender, RoutedEventArgs e)
        {
            var misAsignaturasView = new MisAsignaturasView(EstudianteActual);
            misAsignaturasView.ShowDialog();
        }

        private void BtnDocentes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad de Docentes próximamente...", "K-NECT", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnHorasEstudio_Click(object sender, RoutedEventArgs e)
        {
            var registroHorasView = new RegistroHorasView(EstudianteActual);
            registroHorasView.ShowDialog();
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad de Reportes próximamente...", "K-NECT", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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
            MessageBox.Show("📬 No tienes notificaciones nuevas", "Notificaciones", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAjustes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("⚙️ Configuración próximamente...", "Ajustes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres cerrar sesión?", "Cerrar Sesión",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        // ===== EVENTOS DE TARJETAS =====
        private void CardAsignaturas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BtnAsignaturas_Click(sender, e);
        }

        private void CardCalificaciones_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Ver Calificaciones próximamente...", "K-NECT", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CardDocentes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BtnDocentes_Click(sender, e);
        }
        private void CardHorasEstudio_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //BtnHorasEstudio_Click(sender, e);
        }
    }
}