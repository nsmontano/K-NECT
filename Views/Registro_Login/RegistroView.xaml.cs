using K_NECT.Data;
using K_NECT.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace K_NECT.Views.Registro_Login
{
    /// <summary>
    /// 🎓 Pantalla de registro SOLO para ESTUDIANTES
    /// Los docentes reciben credenciales del administrador
    /// </summary>
    public partial class RegistroView : Window
    {
        public RegistroView()
        {
            InitializeComponent();
            ConfigurarCamposIniciales();
        }

        // ===== CONFIGURACIÓN INICIAL =====
        private void ConfigurarCamposIniciales()
        {
            // Configurar fecha mínima y máxima (16-60 años)
            dpFechaNacimiento.DisplayDateStart = DateTime.Today.AddYears(-60);
            dpFechaNacimiento.DisplayDateEnd = DateTime.Today.AddYears(-16);
            dpFechaNacimiento.SelectedDate = DateTime.Today.AddYears(-20); // Valor por defecto
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
        private void TxtCodigoEstudiante_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtCodigoEstudiante.Text == "Ej: KNEC2025001")
            {
                txtCodigoEstudiante.Text = "";
                txtCodigoEstudiante.Foreground = (Brush)FindResource("White");
            }
        }

        private void TxtCodigoEstudiante_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoEstudiante.Text))
            {
                txtCodigoEstudiante.Text = "Ej: KNEC2025001";
                txtCodigoEstudiante.Foreground = Brushes.Gray;
            }
        }

        private void TxtTelefono_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTelefono.Text == "503-7123-4567")
            {
                txtTelefono.Text = "";
                txtTelefono.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void TxtTelefono_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                txtTelefono.Text = "503-7123-4567";
                txtTelefono.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void TxtCorreo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtCorreo.Text == "ejemplo@itca.edu.sv")
            {
                txtCorreo.Text = "";
                txtCorreo.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void TxtCorreo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                txtCorreo.Text = "ejemplo@itca.edu.sv";
                txtCorreo.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // ===== VALIDACIONES =====
        private bool ValidarFormulario()
        {
            // 1️⃣ Validar código de estudiante
            if (string.IsNullOrWhiteSpace(txtCodigoEstudiante.Text) ||
                txtCodigoEstudiante.Text == "Ej: KNEC2025001")
            {
                MostrarError("❌ Código requerido\n\nEl código de estudiante es obligatorio");
                txtCodigoEstudiante.Focus();
                return false;
            }

            // 2️⃣ Validar nombres
            if (string.IsNullOrWhiteSpace(txtNombres.Text) || txtNombres.Text.Length < 2)
            {
                MostrarError("❌ Nombre inválido\n\nLos nombres deben tener al menos 2 caracteres");
                txtNombres.Focus();
                return false;
            }

            // 3️⃣ Validar apellidos
            if (string.IsNullOrWhiteSpace(txtApellidos.Text) || txtApellidos.Text.Length < 2)
            {
                MostrarError("❌ Apellido inválido\n\nLos apellidos deben tener al menos 2 caracteres");
                txtApellidos.Focus();
                return false;
            }

            // 4️⃣ Validar carrera
            if (string.IsNullOrWhiteSpace(txtCarrera.Text))
            {
                MostrarError("❌ Carrera requerida\n\nPor favor ingresa tu carrera");
                txtCarrera.Focus();
                return false;
            }

            // 5️⃣ Validar semestre
            if (string.IsNullOrWhiteSpace(txtSemestre.Text) ||
                !int.TryParse(txtSemestre.Text, out int semestre) ||
                semestre < 1 || semestre > 12)
            {
                MostrarError("❌ Semestre inválido\n\nEl semestre debe ser un número entre 1 y 12");
                txtSemestre.Focus();
                return false;
            }

            // 6️⃣ Validar correo electrónico
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                txtCorreo.Text == "ejemplo@itca.edu.sv" ||
                !EsEmailValido(txtCorreo.Text))
            {
                MostrarError("❌ Correo inválido\n\nPor favor ingrese un correo electrónico válido");
                txtCorreo.Focus();
                return false;
            }

            // 7️⃣ Validar contraseña
            if (string.IsNullOrWhiteSpace(txtPassword.Password) || txtPassword.Password.Length < 6)
            {
                MostrarError("❌ Contraseña débil\n\nLa contraseña debe tener al menos 6 caracteres");
                txtPassword.Focus();
                return false;
            }

            // 8️⃣ Validar confirmación de contraseña
            if (txtPassword.Password != txtConfirmarPassword.Password)
            {
                MostrarError("❌ Contraseñas no coinciden\n\nLas contraseñas deben ser idénticas");
                txtConfirmarPassword.Focus();
                return false;
            }

            // 9️⃣ Validar términos y condiciones
            if (chkTerminos.IsChecked != true)
            {
                MostrarError("❌ Términos requeridos\n\nDebe aceptar los términos y condiciones");
                return false;
            }

            return true;
        }

        private bool EsEmailValido(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        // ===== BOTÓN: REGISTRARSE =====
        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1️⃣ Validar formulario
                if (!ValidarFormulario())
                    return;

                // 2️⃣ Registrar estudiante
                RegistrarEstudiante();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Error inesperado durante el registro:\n\n{ex.Message}",
                    "Error - K-NECT",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // ===== REGISTRO: ESTUDIANTE =====
        private void RegistrarEstudiante()
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    // 1️⃣ Verificar si el correo ya existe
                    var correoExiste = context.ESTUDIANTE
                        .Any(e => e.CorreoEstudiante.ToLower() == txtCorreo.Text.Trim().ToLower());

                    if (correoExiste)
                    {
                        MostrarError("❌ Correo ya registrado\n\nYa existe un estudiante con este correo electrónico");
                        return;
                    }

                    // 2️⃣ Verificar si el código ya existe
                    var codigoExiste = context.ESTUDIANTE
                        .Any(e => e.CodigoEstudiante.Trim().ToUpper() == txtCodigoEstudiante.Text.Trim().ToUpper());

                    if (codigoExiste)
                    {
                        MostrarError("❌ Código duplicado\n\nYa existe un estudiante con este código");
                        return;
                    }

                    // 3️⃣ Crear nuevo estudiante
                    var nuevoEstudiante = new Models.Estudiante
                    {
                        CodigoEstudiante = txtCodigoEstudiante.Text.Trim().ToUpper(),
                        Nombres = txtNombres.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        CorreoEstudiante = txtCorreo.Text.Trim().ToLower(),
                        Contraseña = txtPassword.Password.Trim(), // ⚠️ En producción: usar hash (BCrypt)
                        Telefono = txtTelefono.Text == "503-7123-4567" ? null : txtTelefono.Text.Trim(),
                        FechaNacimiento = dpFechaNacimiento.SelectedDate,
                        Carrera = txtCarrera.Text.Trim(),
                        Semestre = int.Parse(txtSemestre.Text.Trim()),
                        Activo = true
                    };

                    // 4️⃣ Guardar en base de datos
                    context.ESTUDIANTE.Add(nuevoEstudiante);
                    context.SaveChanges();

                    // 5️⃣ Mostrar mensaje de éxito
                    MostrarExito(
                        $"✅ ¡Registro exitoso!\n\n" +
                        $"🎓 Estudiante: {nuevoEstudiante.NombreCompleto}\n" +
                        $"📧 Correo: {nuevoEstudiante.CorreoEstudiante}\n" +
                        $"🆔 Código: {nuevoEstudiante.CodigoEstudiante}\n" +
                        $"📚 {nuevoEstudiante.Carrera} - Semestre {nuevoEstudiante.Semestre}\n\n" +
                        $"Ya puedes iniciar sesión con tus credenciales."
                    );

                    // 6️⃣ Redirigir al login
                    RedirigirALogin();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                {
                    MostrarError($"❌ Error de base de datos:\n\n{dbEx.InnerException?.Message ?? dbEx.Message}");
                }
                catch (Exception ex)
                {
                    MostrarError($"❌ Error al guardar estudiante:\n\n{ex.Message}");
                }
            }
        }

        // ===== NAVEGACIÓN =====
        private void RedirigirALogin()
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void TxtIniciarSesion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RedirigirALogin();
        }

        // ===== UTILIDADES =====
        private void MostrarError(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Validación - K-NECT",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        private void MostrarExito(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "¡Registro Exitoso! - K-NECT",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}