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
    /// Lógica de interacción para WinAltaAlumno.xaml
    /// </summary>
    public partial class WinAltaAlumno : Window
    {
        private Alumno currentAlumno;

        public WinAltaAlumno()
        {
            InitializeComponent();
            btnLimpiar.IsEnabled = false;

            // Inicializamos DataContext con objeto Alumno para binding
            currentAlumno = new Alumno();
            this.DataContext = currentAlumno;
        }

        //Para limpiar el formulario
        private void clean_formulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtEmail.Text = "";
            btnLimpiar.IsEnabled = false;
            currentAlumno = new Alumno(); //Se inicie otro objeto alumno de lo contrario siempre intentará dar de alta el mismo alumno.
        }
        //Boton para volver al menu
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //Al cerrarse la ventana se dispara el evento Window_Closing que solicita la confirmación
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }
        //Para registrar un alumno
        private void btnRegistrarAlumno_Click(object sender, RoutedEventArgs e)
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
                    "Alta Alumno",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (resultado != MessageBoxResult.Yes)
                {

                    clean_formulario();

                    MessageBox.Show("Se cancelo el registro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                currentAlumno.Alu_DNI = txtDni.Text.Trim();
                currentAlumno.Alu_Nombre = txtNombre.Text.Trim();
                currentAlumno.Alu_Apellido = txtApellido.Text.Trim();
                currentAlumno.Alu_Email = txtEmail.Text.Trim();

                string error =
                currentAlumno["Alu_DNI"] ??
                currentAlumno["Alu_Apellido"] ??
                currentAlumno["Alu_Nombre"] ??
                currentAlumno["Alu_Email"];
                if (error != null)
                {
                    MessageBox.Show("Hay errores en los datos: " + error, "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    if (currentAlumno.Alu_ID == 0)
                    {
                        // Me sirve para ver de que el DNI sea unico
                        if (TrabajarAlumnos.existe_dni(currentAlumno.Alu_DNI))
                        {
                            MessageBox.Show("El DNI ya pertenece a otro alumno.", "DNI duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Inserta un nuevo id
                        int newId = TrabajarAlumnos.insert_alumno(currentAlumno);
                        currentAlumno.Alu_ID = newId;


                        MessageBox.Show("DNI: " + currentAlumno.Alu_DNI + "\n" +
                                        "Nombre: " + currentAlumno.Alu_Nombre + "\n" +
                                        "Apellido: " + currentAlumno.Alu_Apellido + "\n" +
                                        "Email: " + currentAlumno.Alu_Email + "\n\n" +
                                        "Alumno nuevo con ID: " + newId,
                                        "Datos del Alumno",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }

                    //Limpia el formulario luego del alta del alumno
                    clean_formulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
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

                MessageBox.Show("Se cancelo el registro del alumno.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

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
            btnLimpiar.IsEnabled = hayTexto;
        }
    }
}
