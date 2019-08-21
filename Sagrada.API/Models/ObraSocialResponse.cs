using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class ObraSocialResponse {

        
        public int IdObraSocial { get; set; }


        
        public string Nombre { get; set; }

       
        public  List<ObraSocialUsuario> ObraSocialUsuarios { get; set; }

        public  List<ObraSocialMedico> ObraSocialMedicos { get; set; }

        
        public  List<Turno> Turnos { get; set; }

        
        public  List<UsuarioEnEspera> UsuarioEnEsperas { get; set; }

        
        public  List<UsuarioAtendido> UsuarioAtendidos { get; set; }
    }
}