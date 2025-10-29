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
        //Bandera necesaria para que no quiera cerrarse la aplicación cuando ingresamos
        private bool loginExitoso = false;
        public WinWelcome()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            Roles rolAdmin = new Roles(1, "Administrador");
            Roles rolDocente = new Roles(2, "Docente");
            Roles rolRecepcion = new Roles(3, "Recepcion");
            //Usuarios
            Usuario usuario1 = new Usuario(1, "fer", "123", "Aparicio Fernando", 1);
            Usuario usuario2 = new Usuario(2, "flor", "asd", "Choque Florencia", 2);
            Usuario usuario3 = new Usuario(3, "gabriel", "123", "Herrera Gabriel", 3);
            Usuario usuario4 = new Usuario(4, "juan", "zxc", "Guerrero Juan María", 1);
            //Se agrega la referencia para usar las propiedades públicas definidas en el control de usuario.
            string nomUsuario = login.NombreUsuario;
            string contraseña = login.Contraseña;
            Usuario usuarioEncontrado = null;
            if (usuario1.Usu_NombreUsuario == nomUsuario && usuario1.Usu_Contraseña == contraseña)
            {
                usuarioEncontrado = usuario1;
            }
            else if (usuario2.Usu_NombreUsuario == nomUsuario && usuario2.Usu_Contraseña == contraseña)
            {
                usuarioEncontrado = usuario2;
            }
            else if (usuario3.Usu_NombreUsuario == nomUsuario && usuario3.Usu_Contraseña == contraseña)
            {
                usuarioEncontrado = usuario3;
            }
            else if (usuario4.Usu_NombreUsuario == nomUsuario && usuario4.Usu_Contraseña == contraseña)
            {
                usuarioEncontrado = usuario4;
            }

            if (usuarioEncontrado != null)
            {
                MessageBox.Show("Bienvenido/a " + usuarioEncontrado.Usu_NombreUsuario,
                                "Acceso permitido",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
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
