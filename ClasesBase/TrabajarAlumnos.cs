using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarAlumnos
    {
        //CON ESTA FUNCIÓN SE CARGA LA GRILLA PARA ELEGIR EL REGISTRO A MODIFICAR
        public static DataTable list_alumnos()
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Alumno";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            //EJECUTAMOS LA CONSULTA
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //LLENAMOS LOS DATOS DE LA CONSULTA EN EL DATATABLE
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        //CON ESTA FUNCIÓN SE CARGAN LOS TEXTBOX PARA REALIZAR LA MODIFICACIÓN CUANDO SE SELECCIONE UN REGISTRO EN LA GRILLA
        public static Alumno traerAlumnos(int idAlumno) 
        {
            Alumno oAlumno = null;

            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Alumno WHERE alu_ID = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@id", idAlumno);

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read()) 
            {
                oAlumno = new Alumno();
                oAlumno.Alu_DNI = dr["alu_DNI"].ToString();
                oAlumno.Alu_Apellido = dr["alu_Apellido"].ToString();
                oAlumno.Alu_Nombre = dr["alu_Nombre"].ToString();
                oAlumno.Alu_Email = dr["alu_Email"].ToString();
            }
            dr.Close();
            return oAlumno;
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

        //FUNCIÓN PARA REALIZAR LA MODIFICACIÓN DE ALUMNOS
        public static void modificarAlumno(Alumno oAlumno)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Alumno SET alu_DNI = @dni, alu_Apellido = @apellido, alu_Nombre = @nombre, alu_Email = @correo WHERE alu_ID = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@dni", oAlumno.Alu_DNI);
            cmd.Parameters.AddWithValue("@apellido", oAlumno.Alu_Apellido);
            cmd.Parameters.AddWithValue("@nombre", oAlumno.Alu_Nombre);
            cmd.Parameters.AddWithValue("@correo", oAlumno.Alu_Email);
            cmd.Parameters.AddWithValue("@id", oAlumno.Alu_ID);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
