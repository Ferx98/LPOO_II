using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class Curso
    {
        private int cur_ID;

        public int Cur_ID
        {
            get { return cur_ID; }
            set { cur_ID = value; }
        }
        private string cur_Nombre;

        public string Cur_Nombre
        {
            get { return cur_Nombre; }
            set { cur_Nombre = value; }
        }
        private string cur_Descripcion;

        public string Cur_Descripcion
        {
            get { return cur_Descripcion; }
            set { cur_Descripcion = value; }
        }
        private int cur_Cupo;

        public int Cur_Cupo
        {
            get { return cur_Cupo; }
            set { cur_Cupo = value; }
        }
        private DateTime cur_FechaInicio;

        public DateTime Cur_FechaInicio
        {
            get { return cur_FechaInicio; }
            set { cur_FechaInicio = value; }
        }
        private DateTime cur_FechaFin;

        public DateTime Cur_FechaFin
        {
            get { return cur_FechaFin; }
            set { cur_FechaFin = value; }
        }
        private int est_ID;

        public int Est_ID
        {
            get { return est_ID; }
            set { est_ID = value; }
        }
        private int doc_ID;

        public int Doc_ID
        {
            get { return doc_ID; }
            set { doc_ID = value; }
        }
    }
}
