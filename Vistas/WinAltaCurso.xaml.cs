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
    /// Lógica de interacción para WinAltaCurso.xaml
    /// </summary>
    public partial class WinAltaCurso : Window
    {
        public WinAltaCurso()
        {
            InitializeComponent();
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
                txtDescripcion.Text = "";
                txtCupo.Text = "";
                dpFechaInicio.SelectedDate = null;
                dpFechaFin.SelectedDate = null;

                MessageBox.Show("Se cancelo el registro del curso.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        //Para limpiar el formulario
        private void clean_formulario()
        {
           txtNombre.Text = "";
           txtDescripcion.Text = "";
           txtCupo.Text = "";
           dpFechaInicio.SelectedDate = null;
           dpFechaFin.SelectedDate = null;
           btnLimpiar.IsEnabled = false;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            int cupo;
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtCupo.Text) ||
                !int.TryParse(txtCupo.Text, out cupo) ||
                dpFechaInicio.SelectedDate == null ||
                dpFechaFin.SelectedDate == null)
            {
                MessageBox.Show("Complete todos los campos correctamente.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (dpFechaInicio.SelectedDate.Value > dpFechaFin.SelectedDate.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de finalización.", "Error de fechas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else 
            {
                // Confirmación
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Está seguro de que desea registrar el curso?",
                    "Alta Curso",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    // Crear objeto Curso
                    Curso nuevoCurso = new Curso();
                    nuevoCurso.Cur_Nombre = txtNombre.Text;
                    nuevoCurso.Cur_Descripcion = txtDescripcion.Text;
                    nuevoCurso.Cur_Cupo = cupo;
                    nuevoCurso.Cur_FechaInicio = dpFechaInicio.SelectedDate.Value;
                    nuevoCurso.Cur_FechaFin = dpFechaFin.SelectedDate.Value;

                    string mensaje = "Curso registrado:\n" +
                      "Nombre: " + nuevoCurso.Cur_Nombre + "\n" +
                      "Descripción: " + nuevoCurso.Cur_Descripcion + "\n" +
                      "Cupo: " + nuevoCurso.Cur_Cupo + "\n" +
                      "Inicio: " + nuevoCurso.Cur_FechaInicio.ToShortDateString() + "\n" +
                      "Fin: " + nuevoCurso.Cur_FechaFin.ToShortDateString();

                    MessageBox.Show(mensaje, "Alta Curso", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //Al cerrarse la ventana se dispara el evento Window_Closing que solicita la confirmación
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }

        private void CamposCambiaron(object sender, EventArgs e)
        {
            // Verifica si hay texto en los TextBox o fecha seleccionada
            bool hayDatos = !string.IsNullOrWhiteSpace(txtNombre.Text) ||
                            !string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                            !string.IsNullOrWhiteSpace(txtCupo.Text) ||
                            dpFechaInicio.SelectedDate != null ||
                            dpFechaFin.SelectedDate != null;

            // Habilita o deshabilita el botón Limpiar según corresponda
            btnLimpiar.IsEnabled = hayDatos;
        }
    }
}
