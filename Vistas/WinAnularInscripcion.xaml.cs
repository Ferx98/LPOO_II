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
using System.Data;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for WinAnularInscripcion.xaml
    /// </summary>
    public partial class WinAnularInscripcion : Window
    {
        public WinAnularInscripcion()
        {
            InitializeComponent();
            CargarDNIAlumnos();
        }

        private void cmbDNIAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDNIAlumnos.SelectedValue == null)
                return;

            string dni = cmbDNIAlumnos.SelectedValue.ToString();

            var tabla = TrabajarInscripciones.TraerInscripcionesActivasPorAlumno(dni);

            if (tabla.Rows.Count == 0)
            {
                listCursos.ItemsSource = null;
                MessageBox.Show("Este alumno no tiene inscripciones activas.", "Aviso",
                                 MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            listCursos.ItemsSource = tabla.DefaultView;
        }

        private void CargarDNIAlumnos()
        {
            cmbDNIAlumnos.ItemsSource = TrabajarInscripciones.TraerDNIAlumnosInscriptos().DefaultView;
        }

        private void rdbAnular_Checked(object sender, RoutedEventArgs e)
        {
            if (listCursos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una inscripción.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            DataRowView fila = listCursos.SelectedItem as DataRowView;

            int insID = Convert.ToInt32(fila["ins_ID"]);
            int cursoID = Convert.ToInt32(fila["cur_ID"]);

            MessageBoxResult result = MessageBox.Show("¿Desea anular la inscripción seleccionada?",
                                                 "Confirmación",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Cambia el estado del alumno a CANCELADO
                TrabajarInscripciones.AnularInscripcion(insID);

                // Devuelve al curso un cupo
                TrabajarInscripciones.AumentarCupoCurso(cursoID);

                MessageBox.Show("Inscripción anulada con éxito.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                CargarDNIAlumnos();

                // Vacia la grilla
                listCursos.ItemsSource = null;

                //se encarga de desmarcar el RadioButton
                rdbAnular.IsChecked = false;

            }
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

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rdbAnular.IsChecked = false;
        }
    }
}
