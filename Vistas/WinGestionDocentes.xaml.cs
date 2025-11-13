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
    /// Lógica de interacción para WinGestionDocentes.xaml
    /// </summary>
    public partial class WinGestionDocentes : Window
    {
        private ObservableCollection<Curso> listaCursos;
        public WinGestionDocentes()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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

                cmbDocentes.SelectionChanged += cmbDocentes_SelectionChanged;
                listCursos.SelectionChanged += listCursos_SelectionChanged;
                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
            
        }

        private void cmbDocentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDocentes.SelectedValue != null)
            {
                int docId = Convert.ToInt32(cmbDocentes.SelectedValue);
                listaCursos = TrabajarCursos.TraerCursosPorDocente(docId);
                listCursos.ItemsSource = listaCursos;
            }
        }

        private void listCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Curso curso = listCursos.SelectedItem as Curso;
            if (curso != null)
            {
                string estado = curso.EstadoNombre;

                //Se desmarcan los radio Button
                rbdFinalizado.IsChecked = false;
                rdbCancelado.IsChecked = false;

                //Se activan los radio Button de acuerdo al estado
                rbdFinalizado.IsEnabled = (estado == "en_curso");
                rdbCancelado.IsEnabled = (estado == "programado");
            }
        }

        //CAMBIAR ESTADO DE CURSO A FINALIZADO
        private void rbdFinalizado_Checked(object sender, RoutedEventArgs e)
        {
            Curso curso = listCursos.SelectedItem as Curso;
            if (curso != null && curso.EstadoNombre == "en_curso")
            {
                MessageBoxResult result = MessageBox.Show(
                    "¿Está seguro que desea cambiar el estado del curso a FINALIZADO?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    TrabajarCursos.CambiarEstadoCurso(curso, "finalizado");
                    listCursos.Items.Refresh();
                }
                else
                {
                    // Desmarca el RadioButton si el usuario elige "No"
                    rbdFinalizado.IsChecked = false;
                    listCursos.SelectedItem = null;
                }
            }
            else
            {
                rbdFinalizado.IsChecked = false;
            }
        }

        //CAMBIAR ESTADO DE CURSO A CANCELADO
        private void rdbCancelado_Checked(object sender, RoutedEventArgs e)
        {
            Curso curso = listCursos.SelectedItem as Curso;
            if (curso != null && curso.EstadoNombre == "programado")
            {
                MessageBoxResult result = MessageBox.Show(
                    "¿Está seguro que desea cambiar el estado del curso a CANCELADO?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    TrabajarCursos.CambiarEstadoCurso(curso, "cancelado");
                    listCursos.Items.Refresh();
                }
                else
                {
                    // Desmarca el RadioButton si el usuario elige "No"
                    rdbCancelado.IsChecked = false;
                    listCursos.SelectedItem = null;
                }
            }
            else
            {
                rdbCancelado.IsChecked = false;
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
