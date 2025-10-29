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
    /// Lógica de interacción para WinABMUsuarios.xaml
    /// </summary>
    public partial class WinABMUsuarios : Window
    {
        public WinABMUsuarios()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        CollectionView Vista;
        ObservableCollection<Usuario> listaUsuario;

        //ObservableCollection para el comboBox
        ObservableCollection<Roles> listaRoles = new ObservableCollection<Roles>()
        {
            new Roles(){ Rol_ID = 1, Rol_Descripcion = "Administrador" },
            new Roles(){ Rol_ID = 2, Rol_Descripcion = "Docente" },
            new Roles(){ Rol_ID = 3, Rol_Descripcion = "Recepcion" }
        };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Traemos la colección desde el ObjectDataProvider
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_USER"];
            listaUsuario = odp.Data as ObservableCollection<Usuario>;

            // Crear la vista de colección para navegación
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaUsuario);

            // Establece el DataContext en los textBox para mostrar los datos de los usuarios.
            txtUsuNom.DataContext = Vista.CurrentItem;
            txtPassword.DataContext = Vista.CurrentItem;
            txtApeNom.DataContext = Vista.CurrentItem;
            txtRol.DataContext = Vista.CurrentItem;
            //Se carga el comboBox con la lista de roles
            cmbRoles.ItemsSource = listaRoles;
            cmbRoles.DisplayMemberPath = "Rol_Descripcion"; // lo que ve el usuario
            cmbRoles.SelectedValuePath = "Rol_ID";          // lo que se guarda internamente
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
            txtUsuNom.DataContext = Vista.CurrentItem;
            txtPassword.DataContext = Vista.CurrentItem;
            txtApeNom.DataContext = Vista.CurrentItem;
            txtRol.DataContext = Vista.CurrentItem;
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Metodo para guardar un nuevo usuario
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContraseña.Password) ||
                string.IsNullOrWhiteSpace(txtApellidoNombre.Text) ||
                cmbRoles.SelectedItem == null)
            {
                MessageBox.Show("Debe completar todos los campos antes de continuar.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                "¿Esta seguro de que desea registrar este nuevo usuario?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Usuario oUsuario = new Usuario();
                oUsuario.Usu_NombreUsuario = txtNombreUsuario.Text;
                oUsuario.Usu_Contraseña = txtContraseña.Password;
                oUsuario.Usu_ApellidoNombre = txtApellidoNombre.Text;
                oUsuario.Rol_ID = (int)cmbRoles.SelectedValue;

                TrabajarUsuario.insert_usuario(oUsuario);

                MessageBox.Show("Usuario registrado correctamente.",
                                "Éxito",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                load_usuarios();
                clean_formulario();
            }
        }

        //metodo para modificar un usuario
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Seleccione el usuario que desee modificar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }


            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea modificar este usuario?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Usuario oUsuario = (Usuario)Vista.CurrentItem;
                oUsuario.Usu_NombreUsuario = txtNombreUsuario.Text;
                oUsuario.Usu_Contraseña = txtContraseña.Password;
                oUsuario.Usu_ApellidoNombre = txtApellidoNombre.Text;
                oUsuario.Rol_ID = Convert.ToInt32(cmbRoles.SelectedValue);

                TrabajarUsuario.updateUsuario(oUsuario);

                MessageBox.Show("El usuario se modifico correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                load_usuarios();
            }
        }

        //metodo para eliminar un usuario
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CurrentItem == null)
            {
                MessageBox.Show("Seleccione el usuario que desee eliminar.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar este usuario?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Usuario oUsuario = (Usuario)Vista.CurrentItem;
                TrabajarUsuario.deleteUsuario(oUsuario.Usu_ID);
                MessageBox.Show("Usuario eliminado correctamente.",
                                "Eliminación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                load_usuarios();
                clean_formulario();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            clean_formulario();
        }

        //metodo para limpiar todo el formulario
        private void clean_formulario()
        {
            txtNombreUsuario.Text = "";
            txtContraseña.Password = "";
            txtApellidoNombre.Text = "";
            cmbRoles.SelectedIndex = -1;
        }

        private void load_usuarios()
        {
            ObjectDataProvider odp = (ObjectDataProvider)this.Resources["LIST_USER"];
            listaUsuario = odp.Data as ObservableCollection<Usuario>;
            Vista = (CollectionView)CollectionViewSource.GetDefaultView(listaUsuario);
            ActualizarBindings();
        }
    }
}
