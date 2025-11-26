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

        // PROPIEDAD EXTENDIDA PARA MOSTRAR EN LA GRILLA DE LISTA DE INSCRIPCION
        public string Alu_DNI { get; set; }
        public string AlumnoNombreCompleto { get; set; }
        public string CursoNombre { get; set; } // tambien usado para WinResultados
        public string EstadoDescripcion { get; set; }


        //PROPIEDADES AUXILIARES PARA MOSTRAR DATOS EN WinResultados.
        public DateTime CursoFechaFin { get; set; }
        public string DocenteNombre { get; set; }
        
        //PROPIEDAD PARA VALIDAR SI EL CURSO ESTA "en_Curso" PARA SEGUIR EL PROCESO DE ACREDITACION
        public int EstadoCurso { get; set; }
        
        //PROPIEDADES PARA MOSTRAR LA DESCRIPCION DE CADA ESTADO EN LA GRILLA
        public string DescripcionEstadoInscripcion { get; set; } // Ejemplo"Confirmado"
        public string DescripcionEstadoCurso { get; set; }       // Ejemplo "En Curso"
    }
}
