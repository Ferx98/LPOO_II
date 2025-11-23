using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarUsuario
    {
        //FUNCIÓN PARA VALIDAR USUARIOS EN LOGIN
        public static Usuario ValidarUsuario(string nombreUsuario, string contraseña)
        {
            Usuario user = null;

            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = @"
                SELECT usu_ID, usu_NombreUsuario, usu_Contraseña, usu_ApellidoNombre, rol_ID
                FROM Usuario
                WHERE usu_NombreUsuario = @usuario 
                  AND usu_Contraseña = @pass;
            ";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@pass", contraseña);

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    user = new Usuario();
                    user.Usu_ID = Convert.ToInt32(dr["usu_ID"]);
                    user.Usu_NombreUsuario = dr["usu_NombreUsuario"].ToString();
                    user.Usu_Contraseña = dr["usu_Contraseña"].ToString();
                    user.Usu_ApellidoNombre = dr["usu_ApellidoNombre"].ToString();
                    user.Rol_ID = Convert.ToInt32(dr["rol_ID"]);
                }

                dr.Close();
            }

            return user;
        }

        //Método para cargar una coleccion con los usuarios registrados.
        public static ObservableCollection<Usuario> TraerUsuarios() 
        {
            ObservableCollection<Usuario> listaUsuarios = new ObservableCollection<Usuario>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
                              SELECT u.usu_ID,u.usu_NombreUsuario, u.usu_Contraseña, u.usu_ApellidoNombre,r.rol_ID, r.rol_Descripcion
                              FROM Usuario u
                              INNER JOIN Roles r ON u.rol_ID = r.rol_ID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Usuario oUsuario = new Usuario();
                oUsuario.Usu_ID = Convert.ToInt32(dr["usu_ID"]);
                oUsuario.Usu_NombreUsuario = dr["usu_NombreUsuario"].ToString();
                oUsuario.Usu_Contraseña = dr["usu_Contraseña"].ToString();
                oUsuario.Usu_ApellidoNombre = dr["usu_ApellidoNombre"].ToString();
                oUsuario.Rol_ID = Convert.ToInt32(dr["rol_ID"]);
                oUsuario.Rol_Descripcion = dr["rol_descripcion"].ToString();
                listaUsuarios.Add(oUsuario);
            }
            dr.Close();
            cnn.Close();
            
            return listaUsuarios;
        }

        // INSERTAR NUEVO USUARIO
        public static void insert_usuario(Usuario oUsuario)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            // VERIFICA SI EL USUARIO YA EXISTE
            SqlCommand cmdConsulta = new SqlCommand();
            cmdConsulta.Connection = cnn;
            cmdConsulta.CommandText = "SELECT COUNT(*) FROM Usuario WHERE Usu_NombreUsuario = @nombreUsuario";
            cmdConsulta.Parameters.AddWithValue("@nombreUsuario", oUsuario.Usu_NombreUsuario);

            cnn.Open();
            int existe = (int)cmdConsulta.ExecuteScalar();

            if (existe > 0)
            {
                cnn.Close();
                throw new Exception("Ya existe un usuario con ese nombre.");
            }
            //REALIZA LA INSERCION SI NO EXISTE EL USUARIO
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO Usuario(Usu_NombreUsuario, Usu_Contraseña, Usu_ApellidoNombre, Rol_ID) VALUES(@nombreUsuario, @contraseña, @apellidoNombre, @rolID)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@nombreUsuario", oUsuario.Usu_NombreUsuario);
            cmd.Parameters.AddWithValue("@contraseña", oUsuario.Usu_Contraseña);
            cmd.Parameters.AddWithValue("@apellidoNombre", oUsuario.Usu_ApellidoNombre);
            cmd.Parameters.AddWithValue("@rolID", oUsuario.Rol_ID);

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        // MODIFICAR USUARIO EXISTENTE
        public static void updateUsuario(Usuario oUsuario)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            // VERIFICA SI EL USUARIO YA EXISTE
            SqlCommand cmdConsulta = new SqlCommand();
            cmdConsulta.Connection = cnn;
            cmdConsulta.CommandText =
                "SELECT COUNT(*) FROM Usuario " +
                "WHERE Usu_NombreUsuario = @nombreUsuario AND Usu_ID <> @id"; //EXCLUYO AL USUARIO QUE ESTOY MODIFICANDO PARA EVITAR DUPLICACIONES

            cmdConsulta.Parameters.AddWithValue("@nombreUsuario", oUsuario.Usu_NombreUsuario);
            cmdConsulta.Parameters.AddWithValue("@id", oUsuario.Usu_ID);

            cnn.Open();
            int existe = (int)cmdConsulta.ExecuteScalar();

            if (existe > 0)
            {
                cnn.Close();
                throw new Exception("Ya existe un usuario con ese nombre.");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Usuario SET usu_NombreUsuario=@nombreUsuario, usu_Contraseña=@contraseña, usu_ApellidoNombre=@apellidoNombre, rol_ID=@rolID WHERE usu_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", oUsuario.Usu_ID);
            cmd.Parameters.AddWithValue("@nombreUsuario", oUsuario.Usu_NombreUsuario);
            cmd.Parameters.AddWithValue("@contraseña", oUsuario.Usu_Contraseña);
            cmd.Parameters.AddWithValue("@apellidoNombre", oUsuario.Usu_ApellidoNombre);
            cmd.Parameters.AddWithValue("@rolID", oUsuario.Rol_ID);

            cmd.ExecuteNonQuery();
        }

        // ELIMINAR USUARIO
        public static void deleteUsuario(int id)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Usuario WHERE Usu_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", id);

            cnn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
