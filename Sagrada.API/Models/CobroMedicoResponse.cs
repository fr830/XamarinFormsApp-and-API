using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class CobroMedicoResponse {

        
        public int IdCobroMedico { get; set; }


        public int IdMedico { get; set; }


        public int IdEspecialidad { get; set; }



        
        public int Honorarios { get; set; }

        
        public  Medico Medico { get; set; }

        
        public  Especialidad Especialidad { get; set; }

        
        public  List<UsuarioAtendido> UsuarioAtendidos { get; set; }
    }
}