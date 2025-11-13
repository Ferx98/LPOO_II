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

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinPrincipal.xaml
    /// </summary>
    public partial class WinPrincipal : Window
    {
        private bool cerrarSesionDesdeMenu = false; //Bandera necesaria para evitar cierre automático de la aplicación en el momento que se ingrese al menú principal
        public WinPrincipal()
        {
            InitializeComponent();
        }

        //Para cerrar el menú principal y volver al login
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro que desea cerrar sesión?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes) 
            {
                //Se crea la instancia de la ventana del login para poder visualizarla al hacer click.
                cerrarSesionDesdeMenu = true;
                WinWelcome login = new WinWelcome();
                Application.Current.MainWindow = login;
                login.Show();

                this.Close();
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cerrarSesionDesdeMenu) // Si el cierre de la sesión es desde el menú
            {
                cerrarSesionDesdeMenu = false; //se resetea la bandera para que la proxima vez que se entre al menú principal
            }
            else
            {
                // Si se cierra la sesión con la X:
                MessageBoxResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar sesión?",
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
                    // Mostrar login y cerrar esta ventana
                    WinWelcome login = new WinWelcome();
                    login.Show();
                    Application.Current.MainWindow = login;
                }
            }
        }

        //Para abrir el formulario de ABM de usuarios
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinABMUsuarios oWinAltaUsuario = new WinABMUsuarios();
            Application.Current.MainWindow = oWinAltaUsuario;
            oWinAltaUsuario.Show();
            this.Close();
        }

        //Para abrir la ventana que muestra el listado de cursos en una grilla
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            
        }
        
        //Para abrir la ventana de Estados de cursos
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            EstadosDeCursos oEstadosDeCursos = new EstadosDeCursos();
            Application.Current.MainWindow = oEstadosDeCursos;
            oEstadosDeCursos.Show();
            this.Close();
        }

        //Para abrir el formulario de ABM de cursos
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinABMCursos oWinABMCurso = new WinABMCursos();
            Application.Current.MainWindow = oWinABMCurso;
            oWinABMCurso.Show();
            this.Close();
        }


        //Para abrir el formulario de ABM de docentes
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;

            WinABMDocentes oWinABMDocentes = new WinABMDocentes();
            Application.Current.MainWindow = oWinABMDocentes;
            oWinABMDocentes.Show();

            this.Close();
        }

        //Para abrir el formulario de ABM de alumnos
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinABMAlumnos oWinABMAlumnos = new WinABMAlumnos();
            Application.Current.MainWindow = oWinABMAlumnos;
            oWinABMAlumnos.Show();
            this.Close();
        }

        //Para abrir el formulario de modificacion de alumnos
        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            
            this.Close();
        }
        
        //Para abrir el listado de cursos
        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinListaCursos oWinCursos = new WinListaCursos();
            oWinCursos.Show();
            this.Close();
        }

        //Para abrir el listado de usuarios
        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinListaUsuarios oWinUsuarios = new WinListaUsuarios();
            oWinUsuarios.Show();
            this.Close();
        }

        //Para abrir el formulario que permite cambiar el estado de los cursos de los docentes
        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinGestionDocentes oWinGestionDocentes = new WinGestionDocentes();
            oWinGestionDocentes.Show();
            this.Close();
        }
    }
}
