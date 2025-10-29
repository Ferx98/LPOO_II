using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class EstadoType
    {
        private int esty_ID;

        public int Esty_ID
        {
            get { return esty_ID; }
            set { esty_ID = value; }
        }
        private string esty_Nombre;

        public string Esty_Nombre
        {
            get { return esty_Nombre; }
            set { esty_Nombre = value; }
        }
    }
}
