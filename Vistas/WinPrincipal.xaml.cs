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

        //Para abrir el formulario de alta de Usuarios
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinAltaUsuario oWinAltaUsuario = new WinAltaUsuario();
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

        //Para abrir el formulario de alta de cursos
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinAltaCurso oWinAltaCurso = new WinAltaCurso();
            Application.Current.MainWindow = oWinAltaCurso;
            oWinAltaCurso.Show();
            this.Close();
        }


        //Para abrir el formulario de alta de docentes
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;

            WinAltaDocente oWinAltaDocente = new WinAltaDocente();
            Application.Current.MainWindow = oWinAltaDocente;
            oWinAltaDocente.Show();

            this.Close();
        }

        //Para abrir el formulario de alta de alumnos
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinAltaAlumno oWinAltaAlumno = new WinAltaAlumno();
            Application.Current.MainWindow = oWinAltaAlumno;
            oWinAltaAlumno.Show();
            this.Close();
        }

        //Para abrir el formulario de modificacion de alumnos
        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinModificarAlumno oWinModificarAlumno = new WinModificarAlumno();
            Application.Current.MainWindow = oWinModificarAlumno;
            oWinModificarAlumno.Show();
            this.Close();
        }
        
        //Para abrir el listado de cursos
        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            cerrarSesionDesdeMenu = true;
            WinListaCursos oWinCursos = new WinListaCursos();
            //oWinCursos.Show();
            oWinCursos.Show();
            this.Close();
        }
    }
}
