using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarCursos
    {
        public static DataTable traerCursos()
        {
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
                    (d.doc_Apellido + ', ' + d.doc_Nombre) AS Docente,
                    e.est_Nombre AS Estado
                FROM Curso c
                INNER JOIN Docente d ON c.doc_ID = d.doc_ID
                INNER JOIN Estado e ON c.est_ID = e.est_ID
            ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
    }
}
