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
    /// Interaction logic for WinListaCursos.xaml
    /// </summary>
    public partial class WinListaCursos : Window
    {
        public WinListaCursos()
        {
            InitializeComponent();
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAtras_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
