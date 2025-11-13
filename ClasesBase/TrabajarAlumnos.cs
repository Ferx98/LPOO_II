using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace ClasesBase
{
    public class TrabajarAlumnos
    {
        //Método para cargar una coleccion con los alumnos registrados.
        public static ObservableCollection<Alumno> TraerAlumnos()
        {
            ObservableCollection<Alumno> listaAlumnos = new ObservableCollection<Alumno>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Alumno";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Alumno oAlumno = new Alumno();
                oAlumno.Alu_ID = Convert.ToInt32(dr["alu_ID"]);
                oAlumno.Alu_DNI = dr["alu_DNI"].ToString();
                oAlumno.Alu_Apellido = dr["alu_Apellido"].ToString();
                oAlumno.Alu_Nombre = dr["alu_Nombre"].ToString();
                oAlumno.Alu_Email = dr["alu_Email"].ToString();
                listaAlumnos.Add(oAlumno);
            }
            dr.Close();
            cnn.Close();

            return listaAlumnos;
        }

        //ESTA FUNCIÓN CONTROLA DE QUE NO SE REGISTREN DNI DUPLICADOS
        public static bool existe_dni(string dni)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM Alumno WHERE LOWER(Alu_DNI) = @DNI";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@DNI", (dni ?? "").ToLower());

            cnn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            cnn.Close();

            return count > 0;
        }

        //ESTA FUNCIÓN CONTROLA QUE NO HAYA DNI REPETIDOS EN LA MODIFICACIÓN
        public static bool existe_dni_modificacion(string dni, int id)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM Alumno WHERE LOWER(Alu_DNI) = @DNI AND Alu_ID <> @ID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@DNI", (dni ?? "").ToLower());
            cmd.Parameters.AddWithValue("@ID", id);

            cnn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            cnn.Close();

            return count > 0;
        }

        //ESTA FUNCIÓN AGREGA ALUMNOS
        public static int insert_alumno(Alumno alumno)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Alumno (Alu_DNI, Alu_Apellido, Alu_Nombre, Alu_Email) " +
                              "VALUES (@DNI, @APELLIDO, @NOMBRE, @EMAIL)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@DNI", alumno.Alu_DNI ?? "");
            cmd.Parameters.AddWithValue("@APELLIDO", alumno.Alu_Apellido ?? "");
            cmd.Parameters.AddWithValue("@NOMBRE", alumno.Alu_Nombre ?? "");
            cmd.Parameters.AddWithValue("@EMAIL", alumno.Alu_Email ?? "");

            cnn.Open();
            // Ejecuta la inserción
            cmd.ExecuteNonQuery();

            // Recupera el último id
            cmd.CommandText = "SELECT TOP 1 Alu_ID FROM Alumno ORDER BY Alu_ID DESC";
            int newId = Convert.ToInt32(cmd.ExecuteScalar());

            cnn.Close();

            return newId;
        }

        // MODIFICAR ALUMNO EXISTENTE
        public static void updateAlumno(Alumno oAlumno)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Alumno SET alu_DNI=@DNI, alu_Apellido=@apellido, alu_Nombre=@nombre, alu_Email=@email WHERE Alu_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", oAlumno.Alu_ID);
            cmd.Parameters.AddWithValue("@DNI", oAlumno.Alu_DNI);
            cmd.Parameters.AddWithValue("@apellido", oAlumno.Alu_Apellido);
            cmd.Parameters.AddWithValue("@nombre", oAlumno.Alu_Nombre);
            cmd.Parameters.AddWithValue("@email", oAlumno.Alu_Email);

            cnn.Open();
            cmd.ExecuteNonQuery();
        }

        // ELIMINAR ALUMNO
        public static void deleteAlumno(int id)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Alumno WHERE Alu_ID=@id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@id", id);

            cnn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
