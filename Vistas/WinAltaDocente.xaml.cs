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
    /// Lógica de interacción para WinAltaDocente.xaml
    /// </summary>
    public partial class WinAltaDocente : Window
    {
        public WinAltaDocente()
        {
            InitializeComponent();
            btnCancelar.IsEnabled = false;
        }

        //Boton para registrar al nuevo docente
        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDni.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
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
                    "Alta Docente",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (resultado == MessageBoxResult.Yes)
                {
                    Docente oDocente = new Docente();
                    oDocente.Doc_Nombre =txtNombre.Text;
                    oDocente.Doc_Apellido = txtApellido.Text;
                    oDocente.Doc_DNI = txtDni.Text;
                    oDocente.Doc_Email = txtEmail.Text;

                    MessageBox.Show("Nombre: " + oDocente.Doc_Nombre + "\n" +
                                    "Apellido: " + oDocente.Doc_Apellido + "\n" +
                                    "DNI: " + oDocente.Doc_DNI + "\n" +
                                    "Email: " + oDocente.Doc_Email,
                                    "Alta Docente",
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

        //Para limpiar el formulario
        private void clean_formulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtEmail.Text = "";
            btnCancelar.IsEnabled = false;
        }

        //Boton para cancelar el registro del docente y limpiar el formulario
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }

        //Boton para volver al menu
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //Al cerrarse la ventana se dispara el evento Window_Closing que solicita la confirmación
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
                txtApellido.Text = "";
                txtDni.Text = "";
                txtEmail.Text = "";

                MessageBox.Show("Se cancelo el registro del docente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        //Para verificar si alguno de los campos tiene texto. 
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool hayTexto = !string.IsNullOrWhiteSpace(txtNombre.Text) ||
                            !string.IsNullOrWhiteSpace(txtApellido.Text) ||
                            !string.IsNullOrWhiteSpace(txtDni.Text) ||
                            !string.IsNullOrWhiteSpace(txtEmail.Text);

            // Habilitamos o deshabilitamos el botón basado en si hay texto.
            btnCancelar.IsEnabled = hayTexto;
        }
    }
}