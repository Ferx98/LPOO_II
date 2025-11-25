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
    /// Interaction logic for WinInscripcionCursos.xaml
    /// </summary>
    public partial class WinInscripcionCursos : Window
    {
        public WinInscripcionCursos()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        CollectionView Vista;
        ObservableCollection<Curso> listaCurso;
        private int idAlumnoSeleccionado = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Traemos la colección desde el ObjectDataProvider
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_CURSO_PROGRAMADOS"];
            odp.Refresh();
            listaCurso = odp.Data as ObservableCollection<Curso>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaCurso);

            // Vincula los textbox con el registro actual
            txtNom.DataContext = Vista.CurrentItem;
            txtDes.DataContext = Vista.CurrentItem;
            txtCurCupo.DataContext = Vista.CurrentItem;
            txtFechaInicio.DataContext = Vista.CurrentItem;
            txtFechaFin.DataContext = Vista.CurrentItem;
            txtEstado.DataContext = Vista.CurrentItem;
            txtDocente.DataContext = Vista.CurrentItem;

            //btnGuardar.IsEnabled = true;
            //btnCancelar.IsEnabled = true;
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
        }

        //Método para regresar al registro anterior
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToPrevious();
            if (Vista.IsCurrentBeforeFirst) Vista.MoveCurrentToLast();
            ActualizarBindings();
        }

        //Método para pasar al siguiente registro
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToNext();
            if (Vista.IsCurrentAfterLast) Vista.MoveCurrentToFirst();
            ActualizarBindings();
        }

        //Método para pasar al último registro
        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            Vista.MoveCurrentToLast();
            ActualizarBindings();
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


        private void load_cursos()
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_CURSO_PROGRAMADOS"];
            //Fuerza al ObjectDataProvider a volver a ejecutar el método TraerCursos()
            odp.Refresh();
            listaCurso = odp.Data as ObservableCollection<Curso>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaCurso);
            ActualizarBindings();
        }

        //metodo para limpiar todo el formulario
        private void clean_formulario()
        {
            txtDNI.Text = "";
            txtNombreAlumno.Text = "";
            txtApellidoAlumno.Text = "";
            cmbCursosInscribir.SelectedIndex = -1;
        }


        //PROCEDIMIENTO PARA HABILITAR LOS CAMPOS SOLO CUANDO SE SELECCIONE ALGUN RADIO BUTTON.
        private void HabilitarCampos(bool habilitar)
        {
            txtDNI.IsEnabled = habilitar;
            txtNombreAlumno.IsEnabled = habilitar;
            txtApellidoAlumno.IsEnabled = habilitar;
            cmbCursosInscribir.IsEnabled = habilitar;
        }

        private void rbtnAlta_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarCampos(true);
            btnGuardar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            clean_formulario();
        }

        //ALTA DE INSCRIPCION
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            btnGuardar.IsEnabled = true;
            //SE RETIRAN LOS ESTILOS PARA EVITAR SU REPETICION
            bool puedeGuardar = true;

            if (idAlumnoSeleccionado == 0)
            {
                MessageBox.Show("Debe completar los datos del alumno.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                puedeGuardar = false;
            }

            Curso curso = Vista.CurrentItem as Curso;

            if (curso == null)
            {
                MessageBox.Show("Debe de seleccionar un curso.", "Validación de fechas", MessageBoxButton.OK, MessageBoxImage.Error);
                puedeGuardar = false;
            }

            // Validar que no esté ya inscripto
            if (TrabajarInscripciones.VerificarInscripcion(idAlumnoSeleccionado, curso.Cur_ID))
            {
                MessageBox.Show("Este alumno ya está inscripto en este curso.");
                puedeGuardar = false;
            }

            if (puedeGuardar)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea registrar este nuevo curso?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        Inscripcion oInscripcion = new Inscripcion();
                        oInscripcion.Alu_ID = idAlumnoSeleccionado;
                        oInscripcion.Cur_ID = curso.Cur_ID;
                        oInscripcion.Ins_Fecha = DateTime.Now;
                        oInscripcion.Est_ID = 2;

                        TrabajarInscripciones.InsertarInscripcion(oInscripcion);

                        TrabajarCursos.DisminuirCupo(curso.Cur_ID);


                        MessageBox.Show("Inscripcion registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        load_cursos();
                        clean_formulario();
                    }
                    catch (Exception ex)
                    {

                        string msg = ex.Message.ToLower(); //VARIABLE QUE GUARDA EL MENSAJE PARA SABER DONDE ACTIVAR EL BORDE ROJO DE ERROR.

                        MessageBox.Show(ex.Message, "Error al registrar",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }
        }


        //BOTÓN DE ANULAR OPERACIÓN
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }

        private void txtDNI_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtDNI.Text.Length >= 7)  // Dni válido
            {
                Alumno alu = TrabajarAlumnos.TraerAlumnoPorDNI(txtDNI.Text);

                if (alu != null)
                {
                    txtNombreAlumno.Text = alu.Alu_Nombre;
                    txtApellidoAlumno.Text = alu.Alu_Apellido;
                    idAlumnoSeleccionado = alu.Alu_ID; // Guardas el id del alumno
                }
                else
                {
                    txtNombreAlumno.Text = "";
                    txtApellidoAlumno.Text = "";
                    idAlumnoSeleccionado = 0;
                }
            }
        }
    }
}
