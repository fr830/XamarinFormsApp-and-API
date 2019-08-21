using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class ObraSocialUsuarioResponse {

        
        public int IdObraSocial { get; set; }

        
        public int IdUsuario { get; set; }

        
        public int NroAfiliado { get; set; }

        
        public  Usuario Usuario { get; set; }

        
        public  ObraSocial ObraSocial { get; set; }
    }

}