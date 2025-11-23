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
using System.ComponentModel;
namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinVistaPreviaUsuarios.xaml
    /// </summary>
    public partial class WinVistaPreviaUsuarios : Window
    {
        private ICollectionView vistaUsuarios;

        public WinVistaPreviaUsuarios()
        {
            InitializeComponent();
        }

        public WinVistaPreviaUsuarios(ICollectionView vistaFiltrada)
        {
            InitializeComponent();
            this.vistaUsuarios = vistaFiltrada;
            this.Loaded += new RoutedEventHandler(WinVistaPreviaUsuarios_Loaded);
        }

        void WinVistaPreviaUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            //Asignamos la lista que recibimos en el constructor al ItemsSource del ListView que nombramos 'listaUsuariosPreview' en el XAML.
            if (this.vistaUsuarios != null)
            {
                listaUsuariosPreview.ItemsSource = this.vistaUsuarios;
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)DocUsuarios).DocumentPaginator, "Imprimir");
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
