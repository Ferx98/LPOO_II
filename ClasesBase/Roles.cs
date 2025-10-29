using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class Roles
    {
        private int rol_ID;

        public int Rol_ID
        {
            get { return rol_ID; }
            set { rol_ID = value; }
        }
        private string rol_Descripcion;

        public string Rol_Descripcion
        {
            get { return rol_Descripcion; }
            set { rol_Descripcion = value; }
        }

        public Roles(int rolID,string rolDescripcion) 
        {
            this.rol_ID = rolID;
            this.rol_Descripcion = rolDescripcion;
        }

        public Roles() 
        {
            
        }
    }
}
