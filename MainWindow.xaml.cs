using System.Windows;
using System.Windows.Input;

namespace K_NECT
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== PERMITIR ARRASTRAR LA VENTANA =====
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // ===== BOTÓN: INICIAR SESIÓN =====
        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de Login
            var loginView = new Views.Registro_Login.LoginView();
            loginView.Show();

            // Cerrar la ventana actual
            this.Close();
        }

        // ===== BOTÓN: REGISTRARSE =====
        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de Registro
            var registroView = new Views.Registro_Login.RegistroView();
            registroView.Show();

            // Cerrar la ventana actual
            this.Close();
        }

        // ===== BOTÓN: CERRAR APLICACIÓN =====
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "¿Estás seguro de que deseas salir de K-NECT?",
                "Salir - K-NECT",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}