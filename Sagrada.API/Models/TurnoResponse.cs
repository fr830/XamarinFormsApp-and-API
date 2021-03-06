using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class TurnoResponse {

        public int IdTurno { get; set; }

        public int IdMedico { get; set; }

        public int IdEspecialidad { get; set; }
        
        public int IdUsuario { get; set; }

        
        public DateTime Fecha { get; set; }

        public int? IdObraSocial { get; set; }

        public Especialidad Especialidad { get; set; }

        public  ObraSocial ObraSocial { get; set; }


        public  Medico Medico { get; set; }

        
        public  Usuario Usuario { get; set; }
    }
}