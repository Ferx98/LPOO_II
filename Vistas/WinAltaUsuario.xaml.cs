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
    /// Lógica de interacción para WinAltaUsuario.xaml
    /// </summary>
    public partial class WinAltaUsuario : Window
    {
        public WinAltaUsuario()
        {
            InitializeComponent();
            btnLimpiar.IsEnabled = false;
        }

        //Para limpiar el formulario
        private void clean_formulario()
        {
            txtNombre.Text = "";
            txtContraseña.Password = "";
            txtApeNom.Text = "";
            cmbRoles.SelectedIndex = -1;
            btnLimpiar.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea regresar al menú principal?",
                "Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (resultado == MessageBoxResult.Yes)
            {
                txtNombre.Text = "";
                txtContraseña.Password = "";
                txtApeNom.Text = "";
                cmbRoles.SelectedIndex = -1;

                MessageBox.Show("Se cancelo el registro del usuario.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtContraseña.Password) ||
                string.IsNullOrWhiteSpace(txtApeNom.Text) ||
                cmbRoles.SelectedIndex == -1)
            {
                MessageBox.Show("Debe completar todos los campos antes de continuar.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Está seguro de que desea registrar los datos?",
                    "Alta Alumno",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (resultado == MessageBoxResult.Yes)
                {
                    Usuario oUsuario = new Usuario();
                    oUsuario.Usu_NombreUsuario = txtNombre.Text;
                    oUsuario.Usu_Contraseña = txtContraseña.Password;
                    oUsuario.Usu_ApellidoNombre = txtApeNom.Text;
                    string rolSeleccionado = cmbRoles.SelectedItem.ToString();
                    if (rolSeleccionado == "Administrador") 
                    {
                        oUsuario.Rol_ID = 1;
                    }
                    else if (rolSeleccionado == "Docente")
                    {
                        oUsuario.Rol_ID = 2;
                    }
                    else 
                    {
                        oUsuario.Rol_ID = 3;
                    }

                    MessageBox.Show("Nombre de usuario: " + oUsuario.Usu_NombreUsuario + "\n" +
                                    "Apellido y Nombre: " + oUsuario.Usu_ApellidoNombre + "\n" +
                                    "Rol: " + rolSeleccionado ,
                                    "Alta Usuario",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information
                    );
                    //Se limipia el formulario luego del alta.
                    clean_formulario();
                }
                else
                {
                    clean_formulario();

                    MessageBox.Show("Se cancelo el registro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbRoles.Items.Add("Administrador");
            cmbRoles.Items.Add("Docente");
            cmbRoles.Items.Add("Recepcion");
        }

        //CONTROLA SI HAY CAMPOS ESCRITOS PARA HABILITAR EL BOTÓN
        private void VerificarCampos()
        {
            bool hayTexto = !string.IsNullOrWhiteSpace(txtNombre.Text) ||
                   !string.IsNullOrWhiteSpace(txtContraseña.Password) ||
                   !string.IsNullOrWhiteSpace(txtApeNom.Text) ||
                   cmbRoles.SelectedIndex != -1;

            btnLimpiar.IsEnabled = hayTexto;
        }
        private void HabilitarBtnLimpiar(object sender, RoutedEventArgs e)
        {
            VerificarCampos();
        }
    }
}
