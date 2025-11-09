using K_NECT.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace K_NECT.Views.Registro_Login
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            ConfigurarPlaceholders();
        }

        // ===== CONFIGURACIÓN INICIAL =====
        private void ConfigurarPlaceholders()
        {
            txtUsuario.Text = "ejemplo@knect.com";
            txtUsuario.Foreground = System.Windows.Media.Brushes.Gray;
        }

        // ===== EVENTOS DE INTERFAZ =====
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // ===== MANEJO DE PLACEHOLDERS =====
        private void TxtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsuario.Text == "ejemplo@knect.com")
            {
                txtUsuario.Text = "";
                txtUsuario.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void TxtUsuario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "ejemplo@knect.com";
                txtUsuario.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // ===== BOTÓN: INICIAR SESIÓN =====
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1️⃣ VALIDAR CAMPOS VACÍOS
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    txtUsuario.Text == "ejemplo@knect.com")
                {
                    MostrarError("Por favor ingresa tu correo electrónico");
                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MostrarError("Por favor ingresa tu contraseña");
                    txtPassword.Focus();
                    return;
                }

                // 2️⃣ OBTENER DATOS DEL FORMULARIO
                string correo = txtUsuario.Text.Trim().ToLower();
                string contraseña = txtPassword.Password.Trim();

                // Obtener tipo de usuario seleccionado
                string tipoUsuario = "";
                if (cmbTipoUsuario.SelectedItem is ComboBoxItem selectedItem)
                {
                    tipoUsuario = selectedItem.Content.ToString();
                }

                // 3️⃣ VALIDAR CREDENCIALES SEGÚN TIPO DE USUARIO
                if (tipoUsuario.Contains("Estudiante"))
                {
                    IniciarSesionEstudiante(correo, contraseña);
                }
                else if (tipoUsuario.Contains("Docente"))
                {
                    IniciarSesionDocente(correo, contraseña);
                }
                else
                {
                    MostrarError("Por favor selecciona un tipo de usuario");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error inesperado:\n{ex.Message}");
            }
        }

        // ===== VALIDACIÓN: ESTUDIANTE =====
        private void IniciarSesionEstudiante(string correo, string contraseña)
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var estudiante = context.ESTUDIANTE
                        .FirstOrDefault(e => e.CorreoEstudiante.ToLower() == correo && e.Activo);

                    if (estudiante == null)
                    {
                        MostrarError("❌ Correo no encontrado\n\nVerifica que tu correo esté registrado como estudiante.");
                        return;
                    }

                    if (estudiante.Contraseña.Trim() != contraseña)
                    {
                        MostrarError("❌ Contraseña incorrecta\n\nIntenta nuevamente.");
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }

                    // ✅ LOGIN EXITOSO
                    var menuEstudiante = new Estudiante.MenuEstudianteView(estudiante);
                    menuEstudiante.Show();
                    this.Close();

                    // TODO: Aquí abriremos el menú del estudiante
                    // var menuEstudiante = new MenuEstudianteView(estudiante);
                    // menuEstudiante.Show();
                    // this.Close();
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al validar estudiante:\n{ex.Message}");
                }
            }
        }

        // ===== VALIDACIÓN: DOCENTE =====
        private void IniciarSesionDocente(string correo, string contraseña)
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var docente = context.DOCENTE
                        .FirstOrDefault(d => d.CorreoDocente.ToLower() == correo && d.Activo);

                    if (docente == null)
                    {
                        MostrarError("❌ Correo no encontrado\n\nVerifica que tu correo esté registrado como docente.");
                        return;
                    }

                    if (docente.Contraseña.Trim() != contraseña)
                    {
                        MostrarError("❌ Contraseña incorrecta\n\nIntenta nuevamente.");
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }

                    // ✅ LOGIN EXITOSO
                    var menuDocente = new Docente.MenuDocenteView(docente);
                    menuDocente.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al validar docente:\n{ex.Message}");
                }
            }
        }

        // ===== ENLACE: IR A REGISTRO =====
        private void TxtRegistrarse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Abrir ventana de registro
            var registroView = new RegistroView();
            registroView.Show();
            this.Close();
        }

        // ===== UTILIDADES =====
        private void MostrarError(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Error - K-NECT",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }
    }
}