using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ClasesBase
{
    public class Alumno:IDataErrorInfo, INotifyPropertyChanged
    {
        private int alu_ID;

        public int Alu_ID
        {
            get { return alu_ID; }
            
            set { if (alu_ID != value) { alu_ID = value; OnPropertyChanged("Alu_ID"); } }
        }
        private string alu_DNI;

        public string Alu_DNI
        {
            get { return alu_DNI; }
            
            set { if (alu_DNI != value) { alu_DNI = value; OnPropertyChanged("Alu_DNI"); } }
        }
        private string alu_Apellido;

        public string Alu_Apellido
        {
            get { return alu_Apellido; }
            
            set { if (alu_Apellido != value) { alu_Apellido = value; OnPropertyChanged("Alu_Apellido"); } }
        }
        private string alu_Nombre;

        public string Alu_Nombre
        {
            get { return alu_Nombre; }
            
            set { if (alu_Nombre != value) { alu_Nombre = value; OnPropertyChanged("Alu_Nombre"); } }
        }
        private string alu_Email;

        public string Alu_Email
        {
            get { return alu_Email; }
            
            set { if (alu_Email != value) { alu_Email = value; OnPropertyChanged("Alu_Email"); } }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string msg_error = null;
                switch (columnName)
                {
                    case "Alu_ID":
                        if (Alu_ID <= 0) msg_error = "El ID es obligatorio y debe ser mayor que 0.";
                        break;
                    case "Alu_DNI":
                        if (string.IsNullOrWhiteSpace(Alu_DNI)) msg_error = "El DNI es obligatorio.";
                        else if (!Regex.IsMatch(Alu_DNI, @"^\d{6,9}$")) msg_error = "El DNI solo debe de contener numeros.";
                        break;
                    case "Alu_Apellido":
                        if (string.IsNullOrWhiteSpace(Alu_Apellido)) msg_error = "El apellido es obligatorio.";
                        break;
                    case "Alu_Nombre":
                        if (string.IsNullOrWhiteSpace(Alu_Nombre)) msg_error = "El nombre es obligatorio.";
                        break;
                    case "Alu_Email":
                        if (string.IsNullOrWhiteSpace(Alu_Email)) msg_error = "El email es obligatorio.";
                        else if (!Regex.IsMatch(Alu_Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) msg_error = "El email no es valido.";
                        break;
                }
                return msg_error;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
