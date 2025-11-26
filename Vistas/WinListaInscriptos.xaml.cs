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
        private ICollectionView vistaColeccion;

        public WinListaInscriptos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_INSCRIPTOS"];
            odp.Refresh();

            // Capturamos la vista correcta del recurso XAML
            CollectionViewSource cvs = (CollectionViewSource)this.Resources["VISTA_INSCRIPTOS"];
            vistaColeccion = cvs.View;

            // asigancion del filtro
            if (vistaColeccion != null)
            {
                vistaColeccion.Filter = FiltroDNI;
            }
        }

        private bool FiltroDNI(object item)
        {
            if (String.IsNullOrEmpty(txtFiltro.Text)) return true;

            Inscripcion inscripcion = item as Inscripcion;
            if (inscripcion == null) return false;

            string dni = inscripcion.Alu_DNI ?? "";
            return dni.StartsWith(txtFiltro.Text);
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (vistaColeccion != null)
            {
                // Limpiar filtro: Si está vacía, mostramos todo y salimos
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    vistaColeccion.Refresh();
                    return;
                }

                // Un DNI válido tiene al menos 7 dígitos
                if (txtFiltro.Text.Length < 7) 
                {
                    MessageBox.Show("Por favor, ingrese un DNI válido.", 
                                    "DNI Incompleto", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Warning);
                    return; // Importante: Cortamos la ejecución aquí para que no busque nada
                }

                // 3. VALIDACIÓN DE EXISTENCIA (Verificamos si alguien coincide)
                bool existe = false;
        
                foreach (Inscripcion item in vistaColeccion.SourceCollection)
                {
                    // Usamos StartsWith para encontrar coincidencias
                    if (item.Alu_DNI != null && item.Alu_DNI.StartsWith(txtFiltro.Text))
                    {
                        existe = true;
                        break; 
                    }
                }

                // APLICAR FILTRO O MOSTRAR AVISO
                if (existe)
                {
                    vistaColeccion.Refresh();
                }
                else
                {
                    MessageBox.Show("No se encontró ningún alumno con el DNI: " + txtFiltro.Text, 
                                    "Sin Resultados", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                    // No refrescamos la vista para no dejar la tabla vacía
                }
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
                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
