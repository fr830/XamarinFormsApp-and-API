using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class ObraSocialMedicoResponse {


        public int IdObraSocial { get; set; }


        public int IdMedico { get; set; }


        public Medico Medico { get; set; }


        public ObraSocial ObraSocial { get; set; }
    }
}
