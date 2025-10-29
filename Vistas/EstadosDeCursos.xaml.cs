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

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para EstadosDeCursos.xaml
    /// </summary>
    public partial class EstadosDeCursos : Window
    {
        public EstadosDeCursos()
        {
            InitializeComponent();
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
                cmbEstados.SelectedIndex = -1;
                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else 
            {
                e.Cancel = true;
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
