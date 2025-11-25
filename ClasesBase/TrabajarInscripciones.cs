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

        //PARA VERIFICAR SI UN ALUMNO YA SE ENCUENTRA INSCRIPTO EN ALGUN CURSO
        public static bool VerificarInscripcion(int aluId, int curId)
        {
            bool existe = false;
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"
                SELECT COUNT(*)
                FROM Inscripcion
                WHERE alu_ID = @aluId AND cur_Id=@curId";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@aluId", aluId);
            cmd.Parameters.AddWithValue("@curId", curId);

            try
            {
                cnn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                existe = count > 0;
            }
            catch (Exception e)
            {
                throw new Exception("Error al verificar la inscripcion: " + e.Message);
            }
            finally
            {
                cnn.Close();
            }

            return existe;
        }

        public static void InsertarInscripcion(Inscripcion ins)
        {
            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Inscripcion(ins_Fecha, cur_ID, alu_ID, est_ID)
                            VALUES(@fecha, @curso, @alumno, @estado)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@fecha", ins.Ins_Fecha);
                cmd.Parameters.AddWithValue("@curso", ins.Cur_ID);
                cmd.Parameters.AddWithValue("@alumno", ins.Alu_ID);
                cmd.Parameters.AddWithValue("@estado", ins.Est_ID);

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable TraerInscriptos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                                SELECT 
                                    i.ins_ID,
                                    a.alu_DNI,
                                    (a.alu_Apellido + ', ' + a.alu_Nombre) AS Alumno,
                                    c.Cur_ID,
                                    c.cur_Nombre AS Curso,
                                    e.est_Nombre AS Estado,
                                    i.ins_Fecha
                                FROM Inscripcion i
                                INNER JOIN Alumno a ON i.alu_ID = a.Alu_ID
                                INNER JOIN Curso c ON i.cur_ID = c.Cur_ID
                                INNER JOIN Estado e ON i.est_ID = e.est_ID
                             ", cnn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        public static DataTable TraerInscripcionesActivasPorAlumno(string dni)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                        SELECT 
                            i.ins_ID,
                            i.ins_Fecha,
                            c.cur_ID,
                            c.cur_Nombre,
                            a.alu_ID,
                            a.alu_DNI,
                            (a.alu_Apellido + ', ' + a.alu_Nombre) AS Alumno
                        FROM Inscripcion i
                        INNER JOIN Alumno a ON i.alu_ID = a.Alu_ID
                        INNER JOIN Curso c ON i.cur_ID = c.Cur_ID
                        WHERE a.alu_DNI = @dni
                        AND i.est_ID = 2", cnn);

                cmd.Parameters.AddWithValue("@dni", dni);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        public static void AnularInscripcion(int insId)
        {
            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Inscripcion SET est_ID = 4 WHERE ins_ID=@id";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@id", insId);

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void AumentarCupoCurso(int cursoID)
        {
            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                                UPDATE Curso
                                SET Cur_Cupo = Cur_Cupo + 1
                                WHERE Cur_ID = @curso
                            ", cnn);

                cmd.Parameters.AddWithValue("@curso", cursoID);
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable TraerDNIAlumnosInscriptos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                        SELECT DISTINCT 
                            a.alu_DNI
                        FROM Inscripcion i
                        INNER JOIN Alumno a ON i.alu_ID = a.Alu_ID
                        WHERE i.est_ID = 2   -- INSCRIPTO
                        ORDER BY a.alu_DNI
                    ", cnn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------

        // BUSCAR CURSOS POR DNI DEL ALUMNO
        public static ObservableCollection<Inscripcion> TraerInscripcionesPorAlumnoDNI(string dni)
        {
            ObservableCollection<Inscripcion> lista = new ObservableCollection<Inscripcion>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            
            // Agregp c.est_ID as EstadoDelCurso para la validación
            cmd.CommandText = @"
                SELECT i.ins_ID, i.ins_Fecha, i.cur_ID, i.alu_ID, i.est_ID, 
                       c.cur_Nombre, c.cur_FechaFin, c.est_ID as IdEstadoCurso,
                       d.doc_Nombre, d.doc_Apellido,
                       e_ins.est_Nombre as EstadoInscripcionNombre,
                       e_cur.est_Nombre as EstadoCursoNombre
                FROM Inscripcion i
                INNER JOIN Curso c ON i.cur_ID = c.cur_ID
                INNER JOIN Docente d ON c.doc_ID = d.doc_ID
                INNER JOIN Alumno a ON i.alu_ID = a.alu_ID
                INNER JOIN Estado e_ins ON i.est_ID = e_ins.est_ID   -- Join para Inscripción
                INNER JOIN Estado e_cur ON c.est_ID = e_cur.est_ID   -- Join para Curso
                WHERE a.alu_DNI = @dni";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@dni", dni);

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Inscripcion oInscripcion = new Inscripcion();
                oInscripcion.Ins_ID = Convert.ToInt32(dr["ins_ID"]);
                oInscripcion.Ins_Fecha = Convert.ToDateTime(dr["ins_Fecha"]);
                oInscripcion.Cur_ID = Convert.ToInt32(dr["cur_ID"]);
                oInscripcion.Alu_ID = Convert.ToInt32(dr["alu_ID"]);
                oInscripcion.Est_ID = Convert.ToInt32(dr["est_ID"]); // Estado de la inscripción

                // Mapeo de datos auxiliares
                oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                oInscripcion.CursoFechaFin = Convert.ToDateTime(dr["cur_FechaFin"]);
                oInscripcion.DocenteNombre = dr["doc_Nombre"].ToString() + " " + dr["doc_Apellido"].ToString();
                oInscripcion.DescripcionEstadoInscripcion = dr["EstadoInscripcionNombre"].ToString();
                oInscripcion.DescripcionEstadoCurso = dr["EstadoCursoNombre"].ToString();

                // Guardo el estado real del curso para validar luego
                oInscripcion.EstadoCurso = Convert.ToInt32(dr["IdEstadoCurso"]);

                lista.Add(oInscripcion);
            }
            dr.Close();
            cnn.Close();
            return lista;
        }

        //ACREDITAR (Actualizar estado de inscripcion a "CONFIRMADO")
        public static void AcreditarInscripcion(int idInscripcion)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Inscripcion SET est_ID = 6 WHERE ins_ID = @id";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cmd.Parameters.AddWithValue("@id", idInscripcion);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
