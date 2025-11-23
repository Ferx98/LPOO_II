using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace ClasesBase
{
    public class TrabajarInscripciones
    {
        //PARA CARGAR UNA COLECCIÓN DE CURSOS EN LOS QUE SE HA INSCRIPTO UN ALUMNO
        public static ObservableCollection<Inscripcion> TraerInscripcionesPorAlumno(int aluId)
        {
            ObservableCollection<Inscripcion> listaInscripciones = new ObservableCollection<Inscripcion>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmd.CommandText = @"
                SELECT  i.ins_ID,
                        i.ins_Fecha,
                        i.cur_ID,
                        i.alu_ID,
                        i.est_ID,
                        c.cur_Nombre,
                        c.cur_FechaFin,
                        d.doc_Nombre,
                        d.doc_Apellido
                FROM Inscripcion i
                INNER JOIN Curso c ON i.cur_ID = c.cur_ID
                INNER JOIN Docente d ON c.doc_ID = d.doc_ID
                WHERE i.alu_ID = @aluId
                AND i.est_ID = 3";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@aluId", aluId);

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Inscripcion oInscripcion = new Inscripcion();
                oInscripcion.Ins_ID = Convert.ToInt32(dr["ins_ID"]);
                oInscripcion.Ins_Fecha = Convert.ToDateTime(dr["ins_Fecha"].ToString());
                oInscripcion.Cur_ID = Convert.ToInt32(dr["cur_ID"].ToString());
                oInscripcion.Alu_ID = Convert.ToInt32(dr["alu_ID"].ToString());
                oInscripcion.Est_ID = Convert.ToInt32(dr["est_ID"].ToString());
                oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                oInscripcion.CursoFechaFin = Convert.ToDateTime(dr["cur_FechaFin"]);
                oInscripcion.DocenteNombre = dr["doc_Nombre"].ToString() + " " + dr["doc_Apellido"].ToString();
                listaInscripciones.Add(oInscripcion);
            }
            dr.Close();
            cnn.Close();
            return listaInscripciones;
        }

        //FUNCIÓN PARA TRAER LA CANTIDAD DE CURSOS FINALIZADOS Y EN CURSO DE UN ALUMNO.
        public static void ListadoInscripcionesPorAlumno(int aluId, out int finalizados, out int enCurso)
        {
            finalizados = 0;
            enCurso = 0;
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
                SELECT est_ID
                FROM Inscripcion
                WHERE alu_ID = @aluId";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@aluId", aluId);

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int estado = Convert.ToInt32(dr["est_ID"]);

                if (estado == 3)        // Finalizado
                    finalizados++;

                if (estado == 2)        // En curso
                    enCurso++;
            }

            dr.Close();
            cnn.Close();
        }
    }
}
