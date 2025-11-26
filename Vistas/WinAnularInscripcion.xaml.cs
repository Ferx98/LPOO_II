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
using System.Collections.ObjectModel;
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
            CargarDNIs();
        }

        // Carga inicial del Combo
        private void CargarDNIs()
        {
            // Usamos el nuevo método que devuelve Colección de Strings
            cmbDNIAlumnos.ItemsSource = TrabajarInscripciones.TraerDNIAlumnosConInscripcionActiva();
        }

        // Cuando se elige un DNI
        private void cmbDNIAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDNIAlumnos.SelectedItem != null)
            {
                string dni = cmbDNIAlumnos.SelectedItem.ToString();

                // Cargamos la lista
                listCursos.ItemsSource = TrabajarInscripciones.TraerInscripcionesActivasPorDNI(dni);
            }
            else
            {
                listCursos.ItemsSource = null;
            }
        }

        // Botón Anular
        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            // Validar Selección
            if (listCursos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione una inscripción de la lista para anular.",
                                "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Inscripcion inscripcion = (Inscripcion)listCursos.SelectedItem;

            //Confirmacion
            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro que desea ANULAR la inscripción al curso: " + inscripcion.CursoNombre + "?\n" +
                "Esta acción liberará un cupo.",
                "Confirmar Anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 4. Ejecutar Anulación
                    TrabajarInscripciones.AnularInscripcion(inscripcion.Ins_ID);

                    // 5. Devolver Cupo (Importante según teoría)
                    TrabajarInscripciones.AumentarCupoCurso(inscripcion.Cur_ID);

                    MessageBox.Show("Inscripción anulada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recargamr la grilla del alumno actual
                    string dniActual = cmbDNIAlumnos.SelectedItem.ToString();
                    listCursos.ItemsSource = TrabajarInscripciones.TraerInscripcionesActivasPorDNI(dniActual);

                    //Refrescar combo
                    CargarDNIs(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al anular: " + ex.Message);
                }
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Tu lógica de volver al menú...
            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea regresar al menú principal?",
                "Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
