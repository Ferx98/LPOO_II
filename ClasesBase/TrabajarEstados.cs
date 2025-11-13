using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace ClasesBase
{
    public class TrabajarEstados
    {
        //Método para cargar una coleccion con los estados registrados.
        public static ObservableCollection<Estado> TraerEstados()
        {
            ObservableCollection<Estado> listaEstados = new ObservableCollection<Estado>();
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.institutoConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Estado";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Estado oEstado = new Estado();
                oEstado.Est_ID = Convert.ToInt32(dr["est_ID"]);
                oEstado.Est_Nombre = dr["est_Nombre"].ToString();
                oEstado.Esty_ID = Convert.ToInt32(dr["esty_ID"]);
                listaEstados.Add(oEstado);
            }
            dr.Close();
            cnn.Close();

            return listaEstados;
        }
    }
}
