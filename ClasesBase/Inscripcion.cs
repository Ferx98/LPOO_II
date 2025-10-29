using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class Inscripcion
    {
        private int ins_ID;

        public int Ins_ID
        {
            get { return ins_ID; }
            set { ins_ID = value; }
        }
        private DateTime ins_Fecha;

        public DateTime Ins_Fecha
        {
            get { return ins_Fecha; }
            set { ins_Fecha = value; }
        }
        private int cur_ID;

        public int Cur_ID
        {
            get { return cur_ID; }
            set { cur_ID = value; }
        }
        private int alu_ID;

        public int Alu_ID
        {
            get { return alu_ID; }
            set { alu_ID = value; }
        }
        private int est_ID;

        public int Est_ID
        {
            get { return est_ID; }
            set { est_ID = value; }
        }
    }
}
