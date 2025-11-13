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
    /// Lógica de interacción para WinABMAlumnos.xaml
    /// </summary>
    public partial class WinABMAlumnos : Window
    {
        public WinABMAlumnos()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        CollectionView Vista;
        ObservableCollection<Alumno> listaAlumno;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Traemos la colección desde el ObjectDataProvider
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_ALUMNO"];
            listaAlumno = odp.Data as ObservableCollection<Alumno>;

            // Creamos la vista de colección para navegación
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaAlumno);

            // Establece el DataContext en los textBox para mostrar los datos de los alumnos.
            txtNom.DataContext = Vista.CurrentItem;
            txtApe.DataContext = Vista.CurrentItem;
            txtAluEmail.DataContext = Vista.CurrentItem;
            txtAluDNI.DataContext = Vista.CurrentItem;

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
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosAlumnoActual();
        }

        //Método para regresar al registro anterior
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToPrevious();
            if (Vista.IsCurrentBeforeFirst) Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosAlumnoActual();
        }

        //Método para pasar al siguiente registro
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToNext();
            if (Vista.IsCurrentAfterLast) Vista.MoveCurrentToFirst();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosAlumnoActual();
        }

        //Método para pasar al último registro
        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarDatosAlumnoActual();
        }

        //Actualiza el Binding de las propiedades del registro actual
        private void ActualizarBindings()
        {
            txtNom.DataContext = Vista.CurrentItem;
            txtApe.DataContext = Vista.CurrentItem;
            txtAluEmail.DataContext = Vista.CurrentItem;
            txtAluDNI.DataContext = Vista.CurrentItem;
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Método para cargar los datos del alumno seleccionado
        private void CargarDatosAlumnoActual()
        {
            if (Vista != null && Vista.CurrentItem != null)
            {
                Alumno oAlumno = (Alumno)Vista.CurrentItem;

                txtNombre.Text = oAlumno.Alu_Nombre;
                txtApellido.Text = oAlumno.Alu_Apellido;
                txtEmail.Text = oAlumno.Alu_Email;
                txtDNI.Text = oAlumno.Alu_DNI;
            }
            else
            {
                MessageBox.Show("No hay un alumno seleccionado.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void load_alumnos()
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_ALUMNO"];
            //Fuerza al ObjectDataProvider a volver a ejecutar el método TraerAlumnos()
            odp.Refresh();
            listaAlumno = odp.Data as ObservableCollection<Alumno>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaAlumno);
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
            CargarDatosAlumnoActual();
        }

        private void rbtnEliminar_Checked(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            CargarDatosAlumnoActual();
        }

        //ALTA DE ALUMNOS
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool puedeGuardar = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de continuar.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                puedeGuardar = false;
            }

            if (puedeGuardar && TrabajarAlumnos.existe_dni(txtDNI.Text))
            {
                MessageBox.Show("Ya existe un alumno con ese DNI.",
                                "Duplicado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                puedeGuardar = false;
            }

            if (puedeGuardar)
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Está seguro de que desea registrar este nuevo alumno?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    Alumno oAlumno = new Alumno();
                    oAlumno.Alu_Nombre = txtNombre.Text;
                    oAlumno.Alu_Apellido = txtApellido.Text;
                    oAlumno.Alu_Email = txtEmail.Text;
                    oAlumno.Alu_DNI = txtDNI.Text;

                    TrabajarAlumnos.insert_alumno(oAlumno);

                    MessageBox.Show("Alumno registrado correctamente.",
                                    "Éxito",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    load_alumnos();
                    clean_formulario();
                }
            }
        }

        //MODIFICACIÓN DE ALUMNOS
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            bool puedeModificar = true;

            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el alumno que desee modificar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                puedeModificar = false;
            }

            if (puedeModificar)
            {
                Alumno oAlumno = (Alumno)Vista.CurrentItem;

                if (TrabajarAlumnos.existe_dni_modificacion(txtDNI.Text, oAlumno.Alu_ID))
                {
                    MessageBox.Show("Ya existe otro alumno con ese DNI.",
                                    "Duplicado",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    puedeModificar = false;
                }

                if (puedeModificar)
                {
                    MessageBoxResult resultado = MessageBox.Show(
                        "¿Está seguro de que desea modificar este alumno?",
                        "Confirmación",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    if (resultado == MessageBoxResult.Yes)
                    {
                        oAlumno.Alu_Nombre = txtNombre.Text;
                        oAlumno.Alu_Apellido = txtApellido.Text;
                        oAlumno.Alu_Email = txtEmail.Text;
                        oAlumno.Alu_DNI = txtDNI.Text;

                        TrabajarAlumnos.updateAlumno(oAlumno);
                        load_alumnos();

                        MessageBox.Show("El alumno se modificó correctamente.",
                                        "Éxito",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                }
            }
        }

        //ELIMINACIÓN DE ALUMNOS
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el alumno que desee eliminar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar este alumno?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Alumno oAlumno = (Alumno)Vista.CurrentItem;
                TrabajarAlumnos.deleteAlumno(oAlumno.Alu_ID);
                MessageBox.Show("Alumno eliminado correctamente.",
                                "Eliminación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                load_alumnos();
                clean_formulario();
            }
        }

        //BOTÓN DE ANULAR OPERACIÓN
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
