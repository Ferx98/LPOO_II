using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarDocentes
    {
        //Método para cargar una coleccion con los docentes registrados.
        public static ObservableCollection<Docente> TraerDocentes()
        {
            ObservableCollection<Docente> listaDocentes = new ObservableCollection<Docente>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Docente";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Docente oDocente = new Docente();
                oDocente.Doc_ID = Convert.ToInt32(dr["doc_ID"]);
                oDocente.Doc_Nombre = dr["doc_Nombre"].ToString();
                oDocente.Doc_Apellido = dr["doc_Apellido"].ToString();
                oDocente.Doc_Email = dr["doc_Email"].ToString();
                oDocente.Doc_DNI = dr["doc_DNI"].ToString();
                listaDocentes.Add(oDocente);
            }
            dr.Close();
            cnn.Close();

            return listaDocentes;
        }

        // INSERTAR NUEVO DOCENTE
        public static void insert_docente(Docente oDocente)
        {
            verificarDNIAlta(oDocente);
            verificarEmailAlta(oDocente);
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO Docente(Doc_DNI, Doc_Apellido, Doc_Nombre, Doc_Email) VALUES(@DNI, @apellido, @nombre, @email)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@DNI", oDocente.Doc_DNI);
            cmd.Parameters.AddWithValue("@apellido", oDocente.Doc_Apellido);
            cmd.Parameters.AddWithValue("@nombre", oDocente.Doc_Nombre);
            cmd.Parameters.AddWithValue("@email", oDocente.Doc_Email);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        // MODIFICAR DOCENTE EXISTENTE
        public static void updateDocente(Docente oDocente)
        {
            verificarDNIModificar(oDocente);
            verificarEmailModificar(oDocente);
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Docente SET doc_DNI=@DNI, doc_Apellido=@apellido, doc_Nombre=@nombre, doc_Email=@email WHERE doc_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", oDocente.Doc_ID);
            cmd.Parameters.AddWithValue("@DNI", oDocente.Doc_DNI);
            cmd.Parameters.AddWithValue("@apellido", oDocente.Doc_Apellido);
            cmd.Parameters.AddWithValue("@nombre", oDocente.Doc_Nombre);
            cmd.Parameters.AddWithValue("@email", oDocente.Doc_Email);

            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        // ELIMINAR DOCENTE
        public static void deleteDocente(int id)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Docente WHERE Doc_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", id);

            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        //PROCEDIMIENTO PARA VERIFICAR QUE NO HAYA OTRO DOCENTE CON EL MISMO DNI
        public static void verificarDNIAlta(Docente oDocente)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmdConsulta = new SqlCommand();
            cmdConsulta.Connection = cnn;
            cmdConsulta.CommandText = "SELECT COUNT(*) FROM Docente WHERE Doc_DNI = @DNI";
            cmdConsulta.Parameters.AddWithValue("@DNI", oDocente.Doc_DNI);

            cnn.Open();
            int existe = (int)cmdConsulta.ExecuteScalar();
            cnn.Close();

            if (existe > 0)
            {
                throw new Exception("Ya existe un docente con ese DNI.");
            }
        }

        //PROCEDIMIENTO PARA VERIFICAR QUE NO HAYA OTRO DOCENTE CON EL MISMO EMAIL
        public static void verificarEmailAlta(Docente oDocente) 
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmdConsulta = new SqlCommand();
            cmdConsulta.Connection = cnn;
            cmdConsulta.CommandText = "SELECT COUNT(*) FROM Docente WHERE Doc_Email = @email";
            cmdConsulta.Parameters.AddWithValue("@email", oDocente.Doc_Email);

            cnn.Open();
            int existe = (int)cmdConsulta.ExecuteScalar();
            cnn.Close();

            if (existe > 0)
            {
                throw new Exception("Ya existe un docente con ese Email.");
            }
        }
        //PROCEDIMIENTO PARA VERIFICAR QUE NO HAYA OTRO DOCENTE CON EL MISMO DNI AL MODIFICAR
        public static void verificarDNIModificar(Docente oDocente)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;

            cmd.CommandText = "SELECT COUNT(*) FROM Docente WHERE Doc_DNI = @DNI AND Doc_ID <> @ID";
            cmd.Parameters.AddWithValue("@DNI", oDocente.Doc_DNI);
            cmd.Parameters.AddWithValue("@ID", oDocente.Doc_ID);

            cnn.Open();
            int existe = (int)cmd.ExecuteScalar();
            cnn.Close();

            if (existe > 0)
            {
                throw new Exception("Ya existe otro docente con ese DNI.");
            }
        }

        //PROCEDIMIENTO PARA VERIFICAR QUE NO HAY OTRO DOCENTE CON EL MISMO EMAIL AL MODIFICAR
        public static void verificarEmailModificar(Docente oDocente)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;

            cmd.CommandText = "SELECT COUNT(*) FROM Docente WHERE Doc_Email = @Email AND Doc_ID <> @ID";
            cmd.Parameters.AddWithValue("@Email", oDocente.Doc_Email);
            cmd.Parameters.AddWithValue("@ID", oDocente.Doc_ID);

            cnn.Open();
            int existe = (int)cmd.ExecuteScalar();
            cnn.Close();

            if (existe > 0)
            {
                throw new Exception("Ya existe otro docente con ese Email.");
            }
        }

    }
}
