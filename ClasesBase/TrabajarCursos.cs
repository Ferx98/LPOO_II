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
            cmd.CommandText = "SELECT * FROM Curso";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            //EJECUTAMOS LA CONSULTA
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //LLENAMOS LOS DATOS DE LA CONSULTA EN EL DATATABLE
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
