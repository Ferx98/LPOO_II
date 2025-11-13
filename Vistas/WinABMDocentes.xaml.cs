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

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinABMDocentes.xaml
    /// </summary>
    public partial class WinABMDocentes : Window
    {
        public WinABMDocentes()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        CollectionView Vista;
        ObservableCollection<Docente> listaDocente;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Traemos la colección desde el ObjectDataProvider
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_DOCENTE"];
            listaDocente = odp.Data as ObservableCollection<Docente>;

            // Crear la vista de colección para navegación
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaDocente);

            // Establece el DataContext en los textBox para mostrar los datos de los docentes.
            txtNom.DataContext = Vista.CurrentItem;
            txtApe.DataContext = Vista.CurrentItem;
            txtDocEmail.DataContext = Vista.CurrentItem;
            txtDocDNI.DataContext = Vista.CurrentItem;

            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
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

        //Método para ir al primer registro
        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToFirst();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosDocenteActual();
        }

        //Método para regresar al registro anterior
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToPrevious();
            if (Vista.IsCurrentBeforeFirst) Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosDocenteActual();
        }

        //Método para pasar al siguiente registro
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToNext();
            if (Vista.IsCurrentAfterLast) Vista.MoveCurrentToFirst();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosDocenteActual();
        }

        //Método para pasar al último registro
        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosDocenteActual();
        }

        //Actualiza el Binding de las propiedades del registro actual
        private void ActualizarBindings()
        {
            txtNom.DataContext = Vista.CurrentItem;
            txtApe.DataContext = Vista.CurrentItem;
            txtDocEmail.DataContext = Vista.CurrentItem;
            txtDocDNI.DataContext = Vista.CurrentItem;
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Método para cargar los datos del docente seleccionado
        private void CargarDatosDocenteActual()
        {
            if (Vista != null && Vista.CurrentItem != null)
            {
                Docente oDocente = (Docente)Vista.CurrentItem;

                txtNombre.Text = oDocente.Doc_Nombre;
                txtApellido.Text = oDocente.Doc_Apellido;
                txtEmail.Text = oDocente.Doc_Email;
                txtDNI.Text = oDocente.Doc_DNI;
            }
            else
            {
                MessageBox.Show("No hay un docente seleccionado.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //metodo para limpiar todo el formulario
        private void clean_formulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtDNI.Text = "";
        }

        private void load_docentes()
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_DOCENTE"];
            //Fuerza al ObjectDataProvider a volver a ejecutar el método TraerDocentes()
            odp.Refresh();
            listaDocente = odp.Data as ObservableCollection<Docente>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaDocente);
            ActualizarBindings();
        }

        private void rbtnAlta_Checked(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
            clean_formulario();
        }

        private void rbtnModificar_Checked(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnEliminar.IsEnabled = false;
            CargarDatosDocenteActual();
        }

        private void rbtnEliminar_Checked(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            CargarDatosDocenteActual();
        }

        //ALTA DE DOCENTES
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de continuar.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                "¿Esta seguro de que desea registrar este nuevo docente?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Docente oDocente = new Docente();
                oDocente.Doc_Nombre = txtNombre.Text;
                oDocente.Doc_Apellido = txtApellido.Text;
                oDocente.Doc_Email = txtEmail.Text;
                oDocente.Doc_DNI = txtDNI.Text;

                TrabajarDocentes.insert_docente(oDocente);

                MessageBox.Show("Docente registrado correctamente.",
                                "Éxito",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                load_docentes();
                clean_formulario();
            }
        }

        //MODIFICACIÓN DE DOCENTES
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el docente que desee modificar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }


            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea modificar este docente?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Docente oDocente = (Docente)Vista.CurrentItem;
                oDocente.Doc_Nombre = txtNombre.Text;
                oDocente.Doc_Apellido = txtApellido.Text;
                oDocente.Doc_Email = txtEmail.Text;
                oDocente.Doc_DNI = txtDNI.Text;

                TrabajarDocentes.updateDocente(oDocente);
                load_docentes();

                MessageBox.Show("El docente se modifico correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        //ELIMINACIÓN DE DOCENTES
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el docente que desee eliminar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar este docente?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Docente oDocente = (Docente)Vista.CurrentItem;
                TrabajarDocentes.deleteDocente(oDocente.Doc_ID);
                MessageBox.Show("Docente eliminado correctamente.",
                                "Eliminación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                load_docentes();
                clean_formulario();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
            // Se desmarcan los radio buttons
            rbtnAlta.IsChecked = false;
            rbtnModificar.IsChecked = false;
            rbtnEliminar.IsChecked = false;

            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
        }
    }
}
