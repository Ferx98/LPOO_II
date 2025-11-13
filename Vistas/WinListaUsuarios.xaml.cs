using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data; // para el CollectionViewSource
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel; // para el ICollectionView
using ClasesBase;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinListaUsuarios.xaml
    /// </summary>
    public partial class WinListaUsuarios : Window
    {
        private CollectionViewSource vistaColeccionFiltrada; // Variable para mantener la vista de la colección

        public WinListaUsuarios()
        {
            InitializeComponent();
            vistaColeccionFiltrada = Resources["VISTA_USER"] as CollectionViewSource;
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vistaColeccionFiltrada != null)
            {
                vistaColeccionFiltrada.Filter += eventVistaUsuario_Filter;
            }
        }

        private void eventVistaUsuario_Filter(object sender, FilterEventArgs e)
        {
            Usuario usuario = e.Item as Usuario;

            if (usuario.Usu_NombreUsuario.StartsWith(txtFiltro.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
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
    }
}
