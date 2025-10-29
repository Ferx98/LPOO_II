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
using System.Data;
namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para WinModificarAlumno.xaml
    /// </summary>
    public partial class WinModificarAlumno : Window
    {
        public WinModificarAlumno()
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
                txtNombre.Text = "";
                txtApellido.Text = "";
                txtDNI.Text = "";
                txtEmail.Text = "";

                MessageBox.Show("Se cancelo la modificación.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                WinPrincipal menu = new WinPrincipal();
                menu.Show();
            }
            else
            {
                //Se cancela el cierre si se elige "No"
                e.Cancel = true;
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //PARA REALIZAR LA MODIFICACION
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                MessageBoxResult respuesta= MessageBox.Show(
                    "¿Está seguro/a que desea modificar este alumno?",
                    "Confirmar modificación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (respuesta == MessageBoxResult.Yes)
                {
                    // Obtenemos la fila seleccionada
                    DataRowView row = (DataRowView)listView1.SelectedItem;
                    int idAlumno = Convert.ToInt32(row["alu_ID"]);

                    // Creamos un objeto Alumno con los nuevos datos
                    Alumno oAlumno = new Alumno();
                    oAlumno.Alu_ID = idAlumno;
                    oAlumno.Alu_DNI = txtDNI.Text;
                    oAlumno.Alu_Apellido = txtApellido.Text;
                    oAlumno.Alu_Nombre = txtNombre.Text;
                    oAlumno.Alu_Email = txtEmail.Text;

                    string error =
                    oAlumno["Alu_DNI"] ??
                    oAlumno["Alu_Apellido"] ??
                    oAlumno["Alu_Nombre"] ??
                    oAlumno["Alu_Email"];

                    if (error != null)
                    {
                        MessageBox.Show("Hay errores en los datos: " + error,
                                        "Error de validación",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                    else
                    {
                        try
                        {
                            // Verificamos si el DNI ya pertenece a otro alumno
                            if (TrabajarAlumnos.existe_dni_modificacion(oAlumno.Alu_DNI, oAlumno.Alu_ID))
                            {
                                MessageBox.Show("El DNI ya pertenece a otro alumno.",
                                                "DNI duplicado",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Warning);
                            }
                            else
                            {
                                // Si todo está correcto, se modifica el registro
                                TrabajarAlumnos.modificarAlumno(oAlumno);

                                MessageBox.Show("Alumno modificado correctamente",
                                                "Éxito",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Information);

                                // Refresca automáticamente la lista
                                listView1.ItemsSource = TrabajarAlumnos.list_alumnos().DefaultView;

                                // Limpia y deshabilita los controles
                                listView1.SelectedItem = null;
                                txtDNI.Text = "";
                                txtApellido.Text = "";
                                txtNombre.Text = "";
                                txtEmail.Text = "";
                                btnModificar.IsEnabled = false;
                                btnCancelar.IsEnabled = false;
                                txtDNI.IsEnabled = false;
                                txtApellido.IsEnabled = false;
                                txtNombre.IsEnabled = false;
                                txtEmail.IsEnabled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrió un error al modificar: " + ex.Message,
                                            "Error",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                        }
                    }
                }
                else 
                {
                    listView1.SelectedItem = null;
                    txtDNI.Text = "";
                    txtApellido.Text = "";
                    txtNombre.Text = "";
                    txtEmail.Text = "";

                    btnModificar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    txtDNI.IsEnabled = false;
                    txtApellido.IsEnabled = false;
                    txtNombre.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                }
                
            }
            else
            {
                MessageBox.Show("Debe seleccionar un alumno para modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            listView1.SelectedItem = null;
            txtDNI.Text = "";
            txtApellido.Text = "";
            txtNombre.Text = "";
            txtEmail.Text = "";

            btnModificar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                //Guardamos la fila seleccionada y extraemos el ID
                DataRowView row = (DataRowView)listView1.SelectedItem;

                int idAlumno = Convert.ToInt32(row["alu_ID"]);

                //LLAMAMOS AL MÉTODO QUE TRAERÁ LOS DATOS DEL ALUMNO SELECCIONADO
                Alumno oAlumno = TrabajarAlumnos.traerAlumnos(idAlumno);

                // Asignamos el DataContext para que los bindings funcionen
                this.DataContext = oAlumno;


                //Se habilitan los textBox
                txtDNI.IsEnabled = true;
                txtApellido.IsEnabled = true;
                txtNombre.IsEnabled = true;
                txtEmail.IsEnabled = true;

                //Se cargan los datos en los textbox
                txtDNI.Text = oAlumno.Alu_DNI;
                txtApellido.Text = oAlumno.Alu_Apellido;
                txtNombre.Text = oAlumno.Alu_Nombre;
                txtEmail.Text = oAlumno.Alu_Email;
                btnModificar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
            else 
            {
                //CUANDO NO ESTÉ SELECCIONADO NINGÚN REGISTRO SE DESHABILITAN LOS BOTONES Y SE LIMPIAN LOS TEXTBOX
                btnModificar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
                txtDNI.IsEnabled = false;
                txtApellido.IsEnabled = false;
                txtNombre.IsEnabled = false;
                txtEmail.IsEnabled = false;

                txtDNI.Text = "";
                txtApellido.Text = "";
                txtNombre.Text = "";
                txtEmail.Text = "";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView1.ItemsSource = TrabajarAlumnos.list_alumnos().DefaultView;
            //SE DESHABILITAN AL INICIO LOS TEXTBOX
            txtDNI.IsEnabled = false;
            txtApellido.IsEnabled = false;
            txtNombre.IsEnabled = false;
            txtEmail.IsEnabled = false;
        }
    }
}
