using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class EspecialidadResponse {

        
        public int IdEspecialidad { get; set; }


        
        
        public string Nombre { get; set; }

       
        public List<CobroMedico> CobroMedicos { get; set; }
    }
}