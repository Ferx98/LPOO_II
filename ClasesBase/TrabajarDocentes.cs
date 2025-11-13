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
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Docente SET doc_DNI=@DNI, doc_Apellido=@apellido, doc_Nombre=@nombre, doc_Email=@email";
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
    }
}
