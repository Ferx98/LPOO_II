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
    /// Interaction logic for WinAcreditacion.xaml
    /// </summary>
    public partial class WinAcreditacion : Window
    {
        public WinAcreditacion()
        {
            InitializeComponent();
        }

        // BOTÓN BUSCAR
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("Por favor, ingrese un número de DNI.");
                return;
            }

            // Llamamos al método que creamos en el Paso 2
            lsvInscripciones.ItemsSource = TrabajarInscripciones.TraerInscripcionesPorAlumnoDNI(txtDNI.Text);
        }

        // BOTÓN ACREDITAR (Donde está la lógica clave)
        private void btnAcreditar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar que se haya seleccionado un ítem
            if (lsvInscripciones.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un curso de la lista.");
                return;
            }

            Inscripcion itemSeleccionado = (Inscripcion)lsvInscripciones.SelectedItem;

            // ID 2 = EnCurso 
            if (itemSeleccionado.EstadoCurso == 2)
            {
                // Validación extra: No acreditar si ya está confirmado
                if (itemSeleccionado.Est_ID == 6)
                {
                    MessageBox.Show("Este curso ya se encuentra acreditado.");
                    return;
                }

                MessageBoxResult resultado = MessageBox.Show(
                    "¿Está seguro de acreditar al alumno en el curso '" + itemSeleccionado.CursoNombre + "'?",
                    "Confirmar Acreditación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Ejecutamos la actualización
                    TrabajarInscripciones.AcreditarInscripcion(itemSeleccionado.Ins_ID);
                    MessageBox.Show("¡Inscripción acreditada exitosamente!");

                    // Recargamos la lista para ver el cambio de estado (debería cambiar Est. Inscrip a 6)
                    btnBuscar_Click(sender, e);
                }
            }
            else
            {
                // Mensaje de Error si la validación falla
                MessageBox.Show("No se puede acreditar este curso.\n" +
                                "Condición requerida: El curso debe estar 'EnCurso'.\n" +
                                "Estado actual del curso: " + itemSeleccionado.DescripcionEstadoCurso,
                                "Error de Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
