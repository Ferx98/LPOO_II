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

            HabilitarCampos(false);
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

        //PROCEDIMIENTO PARA HABILITAR LOS CAMPOS SOLO CUANDO SE SELECCIONE ALGUN RADIO BUTTON.
        private void HabilitarCampos(bool habilitar)
        {
            txtNombre.IsEnabled = habilitar;
            txtDescripcion.IsEnabled = habilitar;
            txtCupo.IsEnabled = habilitar;
            dtpFechaInicio.IsEnabled = habilitar;
            dtpFechaFin.IsEnabled = habilitar;
            cmbDocentes.IsEnabled = habilitar;
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
            HabilitarCampos(true);
            btnGuardar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
            clean_formulario();
        }

        private void rbtnModificar_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarCampos(true);
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnEliminar.IsEnabled = false;
            CargarCursoActual();
        }

        private void rbtnEliminar_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarCampos(false);
            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            CargarCursoActual();
        }

        //ALTA DE CURSOS
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //SE RETIRAN LOS ESTILOS PARA EVITAR SU REPETICION
            dtpFechaInicio.Tag = null;
            dtpFechaFin.Tag = null;
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
                dtpFechaInicio.Tag = "error";
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha de fin.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                puedeGuardar = false;
            }

            if (puedeGuardar)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea registrar este nuevo curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        Curso oCurso = new Curso();
                        oCurso.Cur_Nombre = txtNombre.Text;
                        oCurso.Cur_Descripcion = txtDescripcion.Text;
                        oCurso.Cur_Cupo = Convert.ToInt32(txtCupo.Text);
                        oCurso.Cur_FechaInicio = dtpFechaInicio.SelectedDate.Value;
                        oCurso.Cur_FechaFin = dtpFechaFin.SelectedDate.Value;
                        oCurso.Doc_ID = Convert.ToInt32(cmbDocentes.SelectedValue);

                        TrabajarCursos.insert_curso(oCurso);

                        MessageBox.Show("Curso registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        load_cursos();
                        clean_formulario();
                    }
                    catch (Exception ex) 
                    {
                        // SE LIMPIAN LOS ESTADOS PREVIOS
                        dtpFechaInicio.Tag = null;
                        dtpFechaFin.Tag = null;

                        string msg = ex.Message.ToLower(); //VARIABLE QUE GUARDA EL MENSAJE PARA SABER DONDE ACTIVAR EL BORDE ROJO DE ERROR.

                        if (msg.Contains("inicio"))
                        {
                            dtpFechaInicio.Tag = "error";
                        }
                        else if (msg.Contains("finalización"))
                        {
                            dtpFechaFin.Tag = "error";
                        }
                        MessageBox.Show(ex.Message, "Error al registrar",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    
                }
            }
        }

        //MODIFICACIÓN DE CURSOS
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            //SE RETIRAN LOS ESTILOS PARA EVITAR SU REPETICION
            dtpFechaInicio.Tag = null;
            dtpFechaFin.Tag = null;
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Elija el curso que desee modificar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Curso oCurso = (Curso)Vista.CurrentItem;
                DateTime hoy = DateTime.Today;
                if (dtpFechaInicio.SelectedDate < hoy)
                {
                    dtpFechaInicio.Tag = "error";
                    MessageBox.Show("La fecha de inicio no puede ser menor a la fecha actual.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else 
                {
                    if (dtpFechaFin.SelectedDate < hoy)
                    {
                        dtpFechaFin.Tag = "error";
                        MessageBox.Show("La fecha de finalización no puede ser menor a la fecha actual.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else 
                    {
                        if (dtpFechaInicio.SelectedDate >= dtpFechaFin.SelectedDate)
                        {
                            dtpFechaInicio.Tag = "error";
                            MessageBox.Show("La fecha de inicio debe ser menor que la fecha de fin.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea modificar este curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (resultado == MessageBoxResult.Yes)
                            {
                                try
                                {
                                    oCurso.Cur_Nombre = txtNombre.Text;
                                    oCurso.Cur_Descripcion = txtDescripcion.Text;
                                    oCurso.Cur_Cupo = Convert.ToInt32(txtCupo.Text);
                                    oCurso.Cur_FechaInicio = dtpFechaInicio.SelectedDate.Value;
                                    oCurso.Cur_FechaFin = dtpFechaFin.SelectedDate.Value;
                                    oCurso.Doc_ID = Convert.ToInt32(cmbDocentes.SelectedValue);

                                    TrabajarCursos.updateCurso(oCurso);

                                    MessageBox.Show("Curso modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                                    load_cursos();
                                }
                                catch (Exception ex)
                                {
                                    // SE LIMPIAN LOS ESTADOS PREVIOS
                                    dtpFechaInicio.Tag = null;
                                    dtpFechaFin.Tag = null;

                                    string msg = ex.Message.ToLower(); //VARIABLE QUE GUARDA EL MENSAJE PARA SABER DONDE ACTIVAR EL BORDE ROJO DE ERROR.
                                    if (msg.Contains("finalización"))
                                    {
                                        dtpFechaFin.Tag = "error";
                                    }
                                    MessageBox.Show(ex.Message, "Error al modificar",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                                }

                            }
                        }
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
            dtpFechaInicio.Tag = null;
            dtpFechaFin.Tag = null;
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
