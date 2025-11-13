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
    /// Lógica de interacción para WinABMCursos.xaml
    /// </summary>
    public partial class WinABMCursos : Window
    {
        public WinABMCursos()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        CollectionView Vista;
        ObservableCollection<Curso> listaCurso;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Traemos la colección desde el ObjectDataProvider
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_CURSO"];
            listaCurso = odp.Data as ObservableCollection<Curso>;

            // Establece el DataContext en los textBox para mostrar los datos de los alumnos.
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaCurso);

            // Vincula los textbox con el registro actual
            txtNom.DataContext = Vista.CurrentItem;
            txtDes.DataContext = Vista.CurrentItem;
            txtCurCupo.DataContext = Vista.CurrentItem;
            txtFechaInicio.DataContext = Vista.CurrentItem;
            txtFechaFin.DataContext = Vista.CurrentItem;
            txtEstado.DataContext = Vista.CurrentItem;
            txtDocente.DataContext = Vista.CurrentItem;

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
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarCursoActual();
        }

        //Método para regresar al registro anterior
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToPrevious();
            if (Vista.IsCurrentBeforeFirst) Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarCursoActual();
        }

        //Método para pasar al siguiente registro
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToNext();
            if (Vista.IsCurrentAfterLast) Vista.MoveCurrentToFirst();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarCursoActual();
        }

        //Método para pasar al último registro
        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToLast();
            ActualizarBindings();
            if (rbtnModificar.IsChecked == true || rbtnEliminar.IsChecked == true) CargarCursoActual();
        }

        //Actualiza el Binding de las propiedades del registro actual
        private void ActualizarBindings()
        {
            txtNom.DataContext = Vista.CurrentItem;
            txtDes.DataContext = Vista.CurrentItem;
            txtCurCupo.DataContext = Vista.CurrentItem;
            txtFechaInicio.DataContext = Vista.CurrentItem;
            txtFechaFin.DataContext = Vista.CurrentItem;
            txtEstado.DataContext = Vista.CurrentItem;
            txtDocente.DataContext = Vista.CurrentItem;
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Método para cargar los datos del curso seleccionado
        private void CargarCursoActual()
        {
            if (Vista != null && Vista.CurrentItem != null)
            {
                Curso oCurso = (Curso)Vista.CurrentItem;

                txtNombre.Text = oCurso.Cur_Nombre;
                txtDescripcion.Text = oCurso.Cur_Descripcion;
                txtCupo.Text = oCurso.Cur_Cupo.ToString();
                dtpFechaInicio.SelectedDate = oCurso.Cur_FechaInicio;
                dtpFechaFin.SelectedDate = oCurso.Cur_FechaFin;
                // Asignamos los valores de los ComboBox
                cmbEstados.SelectedValue = oCurso.Est_ID;
                cmbDocentes.SelectedValue = oCurso.Doc_ID;
                txtEstado.Text = oCurso.EstadoNombre;
                txtDocente.Text = oCurso.DocenteNombreCompleto;
            }
            else
            {
                MessageBox.Show("No hay un curso seleccionado.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //metodo para limpiar todo el formulario
        private void clean_formulario()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtCupo.Text = "";
            dtpFechaInicio.SelectedDate = null;
            dtpFechaFin.SelectedDate = null;
            cmbEstados.SelectedIndex = -1;
            cmbDocentes.SelectedIndex = -1;
        }

        private void load_cursos()
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_CURSO"];
            //Fuerza al ObjectDataProvider a volver a ejecutar el método TraerCursos()
            odp.Refresh();
            listaCurso = odp.Data as ObservableCollection<Curso>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaCurso);
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
            CargarCursoActual();
        }

        private void rbtnEliminar_Checked(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            CargarCursoActual();
        }

        //ALTA DE CURSOS
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool puedeGuardar = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtCupo.Text) ||
                dtpFechaInicio.SelectedDate == null ||
                dtpFechaFin.SelectedDate == null)
            {
                MessageBox.Show("Debe completar todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                puedeGuardar = false;
            }

            if (puedeGuardar && dtpFechaInicio.SelectedDate >= dtpFechaFin.SelectedDate)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha de fin.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                puedeGuardar = false;
            }

            if (puedeGuardar)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea registrar este nuevo curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    Curso oCurso = new Curso();
                    oCurso.Cur_Nombre = txtNombre.Text;
                    oCurso.Cur_Descripcion = txtDescripcion.Text;
                    oCurso.Cur_Cupo = Convert.ToInt32(txtCupo.Text);
                    oCurso.Cur_FechaInicio = dtpFechaInicio.SelectedDate.Value;
                    oCurso.Cur_FechaFin = dtpFechaFin.SelectedDate.Value;
                    oCurso.Est_ID = Convert.ToInt32(cmbEstados.SelectedValue);
                    oCurso.Doc_ID = Convert.ToInt32(cmbDocentes.SelectedValue);

                    TrabajarCursos.insert_curso(oCurso);

                    MessageBox.Show("Curso registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    load_cursos();
                    clean_formulario();
                }
            }
        }

        //MODIFICACIÓN DE CURSOS
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el curso que desee modificar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Curso oCurso = (Curso)Vista.CurrentItem;

                if (dtpFechaInicio.SelectedDate >= dtpFechaFin.SelectedDate)
                {
                    MessageBox.Show("La fecha de inicio debe ser menor que la fecha de fin.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea modificar este curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        oCurso.Cur_Nombre = txtNombre.Text;
                        oCurso.Cur_Descripcion = txtDescripcion.Text;
                        oCurso.Cur_Cupo = Convert.ToInt32(txtCupo.Text);
                        oCurso.Cur_FechaInicio = dtpFechaInicio.SelectedDate.Value;
                        oCurso.Cur_FechaFin = dtpFechaFin.SelectedDate.Value;
                        oCurso.Est_ID = Convert.ToInt32(cmbEstados.SelectedValue);
                        oCurso.Doc_ID = Convert.ToInt32(cmbDocentes.SelectedValue);

                        TrabajarCursos.updateCurso(oCurso);

                        MessageBox.Show("Curso modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        load_cursos();
                    }
                }
            }
        }

        //ELIMINACIÓN DE CURSOS
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el curso que desee eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea eliminar este curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    Curso oCurso = (Curso)Vista.CurrentItem;
                    TrabajarCursos.deleteCurso(oCurso.Cur_ID);
                    MessageBox.Show("Curso eliminado correctamente.", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                    load_cursos();
                    clean_formulario();
                }
            }
        }

        //BOTÓN DE ANULAR OPERACIÓN
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
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
