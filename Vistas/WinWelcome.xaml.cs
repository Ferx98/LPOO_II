using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasesBase;
namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinWelcome.xaml
    /// </summary>
    public partial class WinWelcome : Window
    {
        public static string RolActual;
        //Bandera necesaria para que no quiera cerrarse la aplicación cuando ingresamos
        private bool loginExitoso = false;
        public WinWelcome()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string nomUsuario = login.NombreUsuario;
            string contraseña = login.Contraseña;

            Usuario usuarioEncontrado = TrabajarUsuario.ValidarUsuario(nomUsuario, contraseña);

            if (usuarioEncontrado != null)
            {
                MessageBox.Show("Bienvenido/a " + usuarioEncontrado.Usu_NombreUsuario,
                                "Acceso permitido",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                Sesion.UsuarioLogueado = usuarioEncontrado;//guarda el usuario globalmente
                WinPrincipal menu = new WinPrincipal();
                menu.Show();

                loginExitoso = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.",
                                "Acceso denegado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // cierra la aplicación
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (loginExitoso)
            {
                //Sale de la aplicación.
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "¿Quiere salir de la aplicación?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
