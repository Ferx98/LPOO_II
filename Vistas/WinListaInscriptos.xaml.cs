using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;// para el CollectionViewSource
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel; // para el ICollectionView
using ClasesBase;
using System.Data;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for WinListaInscriptos.xaml
    /// </summary>
    public partial class WinListaInscriptos : Window
    {

        private CollectionViewSource vistaColeccionFiltrada; // Variable para mantener la vista de la colección

        public WinListaInscriptos()
        {
            InitializeComponent();
            vistaColeccionFiltrada = Resources["VISTA_INSCRIPTOS"] as CollectionViewSource;
        }
        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vistaColeccionFiltrada != null)
            {
                vistaColeccionFiltrada.Filter -= eventVistaUsuario_Filter;
                vistaColeccionFiltrada.Filter += eventVistaUsuario_Filter;
                vistaColeccionFiltrada.View.Refresh();
            }
        }


        private void eventVistaUsuario_Filter(object sender, FilterEventArgs e)
        {
            DataRowView fila = e.Item as DataRowView;

            bool aceptado = true;

            // me sirve para filtrar por DNI del alumno
            if (!string.IsNullOrWhiteSpace(txtFiltro.Text))
            {
                string dni = fila["alu_DNI"].ToString();

                if (!dni.StartsWith(txtFiltro.Text))
                    aceptado = false;
            }

            // Me filtra por cursos programados
            if (cmbCursos.SelectedValue != null)
            {
                int cursoID = Convert.ToInt32(cmbCursos.SelectedValue);
                int cursoFila = Convert.ToInt32(fila["cur_ID"]);

                if (cursoID != cursoFila)
                    aceptado = false;
            }

            e.Accepted = aceptado;
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

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView vistaFiltrada = vistaColeccionFiltrada.View;
            WinVistaPreviaUsuarios winVistaPrevia = new WinVistaPreviaUsuarios(vistaFiltrada);
            this.Hide();
            winVistaPrevia.ShowDialog();
            this.Show();
        }

        private void dgInscriptos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vistaColeccionFiltrada != null)
            {
                vistaColeccionFiltrada.Filter -= eventVistaUsuario_Filter;
                vistaColeccionFiltrada.Filter += eventVistaUsuario_Filter;
                vistaColeccionFiltrada.View.Refresh();
            }
        }

    }
}
