using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Practica_Final_PII___1
{
    class Personas
    {
        string apellido, nombres;
        int tipo_documento, documento, tipo_estado_civil, sexo;

        public Personas()
        {
            apellido = nombres = "";
            tipo_documento = documento = tipo_estado_civil = 0;
        }
        public Personas(string apellido, string nombres, int tipo_documento, int documento, int tipo_estado_civil, int sexo)
        {
            this.Apellido = apellido;
            this.Nombres = nombres;
            this.Tipo_documento = tipo_documento;
            this.Documento = documento;
            this.Tipo_estado_civil = tipo_estado_civil;
            this.Sexo = sexo;
        }

        public string Apellido { get => apellido; set => apellido = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public int Tipo_documento { get => tipo_documento; set => tipo_documento = value; }
        public int Documento { get => documento; set => documento = value; }
        public int Tipo_estado_civil { get => tipo_estado_civil; set => tipo_estado_civil = value; }
        public int Sexo { get => sexo; set => sexo = value; }
    }
}
