using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace ClasesBase
{
    public class TrabajarCursos
    {
        //Método para cargar una coleccion con los cursos registrados.
        public static ObservableCollection<Curso> TraerCursos()
        {
            ObservableCollection<Curso> listaCursos = new ObservableCollection<Curso>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
                SELECT 
                    c.cur_ID,
                    c.cur_Nombre,
                    c.cur_Descripcion,
                    c.cur_Cupo,
                    c.cur_FechaInicio,
                    c.cur_FechaFin,
                    c.doc_ID,
                    c.est_ID,
                    (d.doc_Apellido + ', ' + d.doc_Nombre) AS Docente,
                    e.est_Nombre AS Estado
                FROM Curso c
                INNER JOIN Docente d ON c.doc_ID = d.doc_ID
                INNER JOIN Estado e ON c.est_ID = e.est_ID
            ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Curso oCurso = new Curso();
                oCurso.Cur_ID = Convert.ToInt32(dr["cur_ID"]);
                oCurso.Cur_Nombre = dr["cur_Nombre"].ToString();
                oCurso.Cur_Descripcion = dr["cur_Descripcion"].ToString();
                oCurso.Cur_Cupo = Convert.ToInt32(dr["cur_Cupo"].ToString());
                oCurso.Cur_FechaInicio = Convert.ToDateTime(dr["cur_FechaInicio"].ToString());
                oCurso.Cur_FechaFin = Convert.ToDateTime(dr["cur_FechaFin"].ToString());
                oCurso.Est_ID = Convert.ToInt32(dr["est_ID"].ToString());
                oCurso.Doc_ID = Convert.ToInt32(dr["doc_ID"].ToString());
                //Se asginan estos campos para mostrar en el ABM Cursos los nombres del docente y del estado.
                oCurso.DocenteNombreCompleto = dr["Docente"].ToString();
                oCurso.EstadoNombre = dr["Estado"].ToString();
                listaCursos.Add(oCurso);
            }
            dr.Close();
            cnn.Close();

            return listaCursos;
        }

        // INSERTAR NUEVO CURSO
        public static int insert_curso(Curso curso)
        {
            // Validación de fechas
            if (curso.Cur_FechaInicio >= curso.Cur_FechaFin)
            {
                throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"
            INSERT INTO Curso (cur_Nombre, cur_Descripcion, cur_Cupo, cur_FechaInicio, cur_FechaFin, doc_ID, est_ID)
            VALUES (@NOMBRE, @DESCRIPCION, @CUPO, @FECHAINICIO, @FECHAFIN, @DOCID, @ESTID)
        ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@NOMBRE", curso.Cur_Nombre ?? "");
            cmd.Parameters.AddWithValue("@DESCRIPCION", curso.Cur_Descripcion ?? "");
            cmd.Parameters.AddWithValue("@CUPO", curso.Cur_Cupo);
            cmd.Parameters.AddWithValue("@FECHAINICIO", curso.Cur_FechaInicio);
            cmd.Parameters.AddWithValue("@FECHAFIN", curso.Cur_FechaFin);
            cmd.Parameters.AddWithValue("@DOCID", curso.Doc_ID);
            cmd.Parameters.AddWithValue("@ESTID", curso.Est_ID);

            cnn.Open();
            cmd.ExecuteNonQuery();

            // Recupera el último ID insertado
            cmd.CommandText = "SELECT TOP 1 cur_ID FROM Curso ORDER BY cur_ID DESC";
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cnn.Close();

            return newId;
        }

        // MODIFICAR CURSO EXISTENTE
        public static void updateCurso(Curso curso)
        {
            if (curso.Cur_FechaInicio >= curso.Cur_FechaFin)
            {
                throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
            UPDATE Curso 
            SET cur_Nombre=@NOMBRE, 
                cur_Descripcion=@DESCRIPCION, 
                cur_Cupo=@CUPO, 
                cur_FechaInicio=@FECHAINICIO, 
                cur_FechaFin=@FECHAFIN, 
                doc_ID=@DOCID, 
                est_ID=@ESTID
            WHERE cur_ID=@ID
        ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@ID", curso.Cur_ID);
            cmd.Parameters.AddWithValue("@NOMBRE", curso.Cur_Nombre);
            cmd.Parameters.AddWithValue("@DESCRIPCION", curso.Cur_Descripcion);
            cmd.Parameters.AddWithValue("@CUPO", curso.Cur_Cupo);
            cmd.Parameters.AddWithValue("@FECHAINICIO", curso.Cur_FechaInicio);
            cmd.Parameters.AddWithValue("@FECHAFIN", curso.Cur_FechaFin);
            cmd.Parameters.AddWithValue("@DOCID", curso.Doc_ID);
            cmd.Parameters.AddWithValue("@ESTID", curso.Est_ID);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        // ELIMINAR CURSO
        public static void deleteCurso(int id)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Curso WHERE cur_ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@ID", id);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        //PARA CARGAR UNA COLECCIÓN DE CURSOS DICTADOS POR UN DOCENTE
        public static ObservableCollection<Curso> TraerCursosPorDocente(int docId)
        {
            ObservableCollection<Curso> listaCursos = new ObservableCollection<Curso>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
                SELECT c.cur_ID, c.cur_Nombre, c.cur_Descripcion, c.cur_Cupo, c.cur_FechaInicio, c.cur_FechaFin, e.est_Nombre
                FROM Curso c
                INNER JOIN Estado e ON c.est_ID = e.est_ID
                WHERE c.doc_ID = @docId";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@docId", docId);

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Curso oCurso = new Curso();
                oCurso.Cur_ID = Convert.ToInt32(dr["cur_ID"]);
                oCurso.Cur_Nombre = dr["cur_Nombre"].ToString();
                oCurso.Cur_Descripcion = dr["cur_Descripcion"].ToString();
                oCurso.Cur_Cupo = Convert.ToInt32(dr["cur_Cupo"].ToString());
                oCurso.Cur_FechaInicio = Convert.ToDateTime(dr["cur_FechaInicio"].ToString());
                oCurso.Cur_FechaFin = Convert.ToDateTime(dr["cur_FechaFin"].ToString());
                oCurso.EstadoNombre = dr["est_Nombre"].ToString();
                listaCursos.Add(oCurso);
            }
            dr.Close();
            cnn.Close();
            return listaCursos;
        }

        public static void CambiarEstadoCurso(Curso curso, string nuevoEstado)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand(@"
                UPDATE Curso 
                SET est_ID = (SELECT est_ID FROM Estado WHERE est_Nombre = @nuevoEstado)
                WHERE cur_ID = @id", cnn);
            cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
            cmd.Parameters.AddWithValue("@id", curso.Cur_ID);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            curso.EstadoNombre = nuevoEstado;
        }
    }
}
