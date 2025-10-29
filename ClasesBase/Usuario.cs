using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClasesBase
{
    public class Usuario : INotifyPropertyChanged
    {
        private int usu_ID;

        public int Usu_ID
        {
            get { return usu_ID; }
            set { usu_ID = value;
                  OnPropertyChanged("Usu_ID");
                }
        }
        private string usu_NombreUsuario;

        public string Usu_NombreUsuario
        {
            get { return usu_NombreUsuario; }
            set { usu_NombreUsuario = value;
                  OnPropertyChanged("Usu_NombreUsuario");
            }
        }
        private string usu_Contraseña;

        public string Usu_Contraseña
        {
            get { return usu_Contraseña; }
            set { usu_Contraseña = value;
            OnPropertyChanged("Usu_Contraseña");
            }
        }
        private string usu_ApellidoNombre;

        public string Usu_ApellidoNombre
        {
            get { return usu_ApellidoNombre; }
            set { usu_ApellidoNombre = value;
            OnPropertyChanged("Usu_ApellidoNombre");
            }
        }
        private int rol_ID;

        public int Rol_ID
        {
            get { return rol_ID; }
            set { rol_ID = value; 
                OnPropertyChanged("Rol_ID"); 
                }
        }

        private string rol_Descripcion;
        public string Rol_Descripcion
        {
            get {return rol_Descripcion; }
            set
            {
                rol_Descripcion = value;
                OnPropertyChanged("Rol_Descripcion");
            }
        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string this[string columnName]
        {
            get
            {
                string msg_error = null;

                switch (columnName)
                {
                    case "Usu_NombreUsuario":
                        if (string.IsNullOrWhiteSpace(Usu_NombreUsuario))
                            msg_error = "El nombre de usuario es obligatorio.";
                        break;

                    case "Usu_Contraseña":
                        if (string.IsNullOrWhiteSpace(Usu_Contraseña))
                            msg_error = "La contraseña es obligatoria.";
                        break;

                    case "Usu_ApellidoNombre":
                        if (string.IsNullOrWhiteSpace(Usu_ApellidoNombre))
                            msg_error = "Debe ingresar el apellido y el nombre del usuario.";
                        break;

                    case "Rol_ID":
                        if (Rol_ID <= 0)
                            msg_error = "Debe de seleccionar un rol.";
                        break;
                }

                return msg_error;
            }
        }

        //CONSTRUCTOR POR DEFECTO
        public Usuario()
        {
        }
        public Usuario(int usuID, string usuNombre, string usuContraseña, string usuApeNom, int rolID)
        {
            this.usu_ID = usuID;
            this.usu_NombreUsuario = usuNombre;
            this.usu_Contraseña = usuContraseña;
            this.usu_ApellidoNombre = usuApeNom;
            this.rol_ID = rolID;
        }
    }
}
