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
using System.Collections.ObjectModel;
namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinResultados.xaml
    /// </summary>
    public partial class WinResultados : Window
    {
        private ObservableCollection<Inscripcion> listaInscripciones;
        public WinResultados()
        {
            InitializeComponent();
        }

        private void cmbAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAlumnos.SelectedValue != null)
            {
                int finalizados;
                int enCurso;
                int aluId = Convert.ToInt32(cmbAlumnos.SelectedValue);
                // Se carga la lista de cursos en los que se ha inscripto el alumno
                listaInscripciones = TrabajarInscripciones.TraerInscripcionesPorAlumno(aluId);
                listInscripciones.ItemsSource = listaInscripciones;
                TrabajarInscripciones.ListadoInscripcionesPorAlumno(aluId, out finalizados, out enCurso);
                // Mostramos en los TextBox las cantidades obtenidas
                txtFinalizados.Text = finalizados.ToString();
                txtEnCurso.Text = enCurso.ToString();
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

                cmbAlumnos.SelectionChanged += cmbAlumnos_SelectionChanged;
                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }
    }
}
