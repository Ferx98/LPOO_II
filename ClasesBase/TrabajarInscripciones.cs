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

        public static void InsertarInscripcion(Inscripcion ins)
        {
            SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString);
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

        public static bool VerificarInscripcion(int aluId, int curId)
        {
            bool existe = false;
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT COUNT(*) FROM Inscripcion WHERE alu_ID = @aluId AND cur_Id=@curId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;
                cmd.Parameters.AddWithValue("@aluId", aluId);
                cmd.Parameters.AddWithValue("@curId", curId);

                cnn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                existe = count > 0;
            }
            return existe;
        }

        public static ObservableCollection<Inscripcion> TraerInscripcionesColeccion()
        {
            ObservableCollection<Inscripcion> lista = new ObservableCollection<Inscripcion>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                    SELECT i.ins_ID, i.ins_Fecha, i.alu_ID, i.cur_ID, i.est_ID,
                            a.alu_DNI,
                            (a.alu_Apellido + ', ' + a.alu_Nombre) as Alumno,
                            c.cur_Nombre,
                            e.est_Nombre
                    FROM Inscripcion i
                    INNER JOIN Alumno a ON i.alu_ID = a.alu_ID
                    INNER JOIN Curso c ON i.cur_ID = c.Cur_ID
                    INNER JOIN Estado e ON i.est_ID = e.est_ID";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Inscripcion oInscripcion = new Inscripcion();
                    oInscripcion.Ins_ID = Convert.ToInt32(dr["ins_ID"]);
                    oInscripcion.Ins_Fecha = Convert.ToDateTime(dr["ins_Fecha"]);
                    oInscripcion.Alu_ID = Convert.ToInt32(dr["alu_ID"]);
                    oInscripcion.Cur_ID = Convert.ToInt32(dr["cur_ID"]);
                    oInscripcion.Est_ID = Convert.ToInt32(dr["est_ID"]);

                    oInscripcion.Alu_DNI = dr["alu_DNI"].ToString();
                    oInscripcion.AlumnoNombreCompleto = dr["Alumno"].ToString();
                    oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                    oInscripcion.EstadoDescripcion = dr["est_Nombre"].ToString();

                    lista.Add(oInscripcion);
                }
            }
            return lista;
        }

        public static ObservableCollection<string> TraerDNIAlumnosConInscripcionActiva()
        {
            ObservableCollection<string> listaDNI = new ObservableCollection<string>();
            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT DISTINCT a.alu_DNI
                    FROM Inscripcion i
                    INNER JOIN Alumno a ON i.alu_ID = a.Alu_ID
                    WHERE i.est_ID = 5  -- 5 = INSCRIPTO
                    ORDER BY a.alu_DNI", cnn);

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listaDNI.Add(dr["alu_DNI"].ToString());
                }
            }
            return listaDNI;
        }

        public static ObservableCollection<Inscripcion> TraerInscripcionesActivasPorDNI(string dni)
        {
            ObservableCollection<Inscripcion> lista = new ObservableCollection<Inscripcion>();
            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT i.ins_ID, i.ins_Fecha, 
                           c.cur_ID, c.cur_Nombre, 
                           a.alu_ID, (a.alu_Apellido + ', ' + a.alu_Nombre) AS Alumno
                    FROM Inscripcion i
                    INNER JOIN Alumno a ON i.alu_ID = a.Alu_ID
                    INNER JOIN Curso c ON i.cur_ID = c.Cur_ID
                    WHERE a.alu_DNI = @dni 
                    AND i.est_ID = 5", cnn); // 5 = INSCRIPTO

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

                    oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                    oInscripcion.AlumnoNombreCompleto = dr["Alumno"].ToString();

                    lista.Add(oInscripcion);
                }
            }
            return lista;
        }

        public static void AnularInscripcion(int insId)
        {
            SqlConnection cnn = new SqlConnection(Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Inscripcion SET est_ID = 4 WHERE ins_ID=@id"; // 4 = CANCELADO
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
                SqlCommand cmd = new SqlCommand(@"UPDATE Curso SET Cur_Cupo = Cur_Cupo + 1 WHERE Cur_ID = @curso", cnn);
                cmd.Parameters.AddWithValue("@curso", cursoID);
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Inscripcion> TraerInscripcionesPorAlumnoDNI(string dni)
        {
            ObservableCollection<Inscripcion> lista = new ObservableCollection<Inscripcion>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
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
                    INNER JOIN Estado e_ins ON i.est_ID = e_ins.est_ID   
                    INNER JOIN Estado e_cur ON c.est_ID = e_cur.est_ID   
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
                    oInscripcion.Est_ID = Convert.ToInt32(dr["est_ID"]);

                    oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                    oInscripcion.CursoFechaFin = Convert.ToDateTime(dr["cur_FechaFin"]);
                    oInscripcion.DocenteNombre = dr["doc_Nombre"].ToString() + " " + dr["doc_Apellido"].ToString();
                    oInscripcion.DescripcionEstadoInscripcion = dr["EstadoInscripcionNombre"].ToString();
                    oInscripcion.DescripcionEstadoCurso = dr["EstadoCursoNombre"].ToString();
                    oInscripcion.EstadoCurso = Convert.ToInt32(dr["IdEstadoCurso"]);

                    lista.Add(oInscripcion);
                }
            }
            return lista;
        }

        public static void AcreditarInscripcion(int idInscripcion)
        {
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Inscripcion SET est_ID = 6 WHERE ins_ID = @id"; // 6 = CONFIRMADO
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;
                cmd.Parameters.AddWithValue("@id", idInscripcion);
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Inscripcion> TraerInscripcionesPorAlumno(int aluId)
        {
            ObservableCollection<Inscripcion> listaInscripciones = new ObservableCollection<Inscripcion>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                    SELECT  i.ins_ID, i.ins_Fecha, i.cur_ID, i.alu_ID, i.est_ID,
                            c.cur_Nombre, c.cur_FechaFin,
                            d.doc_Nombre, d.doc_Apellido
                    FROM Inscripcion i
                    INNER JOIN Curso c ON i.cur_ID = c.cur_ID
                    INNER JOIN Docente d ON c.doc_ID = d.doc_ID
                    WHERE i.alu_ID = @aluId
                    AND i.est_ID = 3"; // 3 = FINALIZADO

                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;
                cmd.Parameters.AddWithValue("@aluId", aluId);

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Inscripcion oInscripcion = new Inscripcion();
                    oInscripcion.Ins_ID = Convert.ToInt32(dr["ins_ID"]);
                    oInscripcion.Ins_Fecha = Convert.ToDateTime(dr["ins_Fecha"]);
                    oInscripcion.Cur_ID = Convert.ToInt32(dr["cur_ID"]);
                    oInscripcion.Alu_ID = Convert.ToInt32(dr["alu_ID"]);
                    oInscripcion.Est_ID = Convert.ToInt32(dr["est_ID"]);
                    oInscripcion.CursoNombre = dr["cur_Nombre"].ToString();
                    oInscripcion.CursoFechaFin = Convert.ToDateTime(dr["cur_FechaFin"]);
                    oInscripcion.DocenteNombre = dr["doc_Nombre"].ToString() + " " + dr["doc_Apellido"].ToString();
                    listaInscripciones.Add(oInscripcion);
                }
            }
            return listaInscripciones;
        }

        public static void ListadoInscripcionesPorAlumno(int aluId, out int finalizados, out int enCurso)
        {
            finalizados = 0;
            enCurso = 0;
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT est_ID FROM Inscripcion WHERE alu_ID = @aluId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;
                cmd.Parameters.AddWithValue("@aluId", aluId);

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int estado = Convert.ToInt32(dr["est_ID"]);
                    if (estado == 3) finalizados++;
                    if (estado == 5) enCurso++; // 5 = Inscripto/En Curso
                }
            }
        }
    }
}