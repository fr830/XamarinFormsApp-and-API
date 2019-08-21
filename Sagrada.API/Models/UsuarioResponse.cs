using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class UsuarioResponse {

        public int IdUsuario { get; set; }



        public string Nombre { get; set; }



        public string Apellido { get; set; }


        public int DNI { get; set; }

        public DateTime FechaDeNacimiento { get; set; }


        public string Email { get; set; }


        public string Telefono { get; set; }


        public string PathImagen { get; set; }

        public int IdTipoUsuario { get; set; }


        public  TipoUsuario TipoUsuario { get; set; }


        public byte[] ImageArray { get; set; }


        public string Password { get; set; }


        public string FullPathImagen {

            get {
                if (string.IsNullOrEmpty(PathImagen)) {
                    return "noimage";
                }

                return string.Format(
                    "http://sagradaapi.azurewebsites.net/{0}",
                    PathImagen.Substring(1));
            }
        }


        public string FullName {

            get {
                return string.Format("{0} {1}", this.Nombre, this.Apellido);
            }
        }


        public  List<UsuarioEnEspera> UsuarioEnEsperas { get; set; }


        public  List<Turno> Turnos { get; set; }


        public  List<Pago> Pagos { get; set; }


        public  List<UsuarioAtendido> UsuarioAtendidos { get; set; }


        public  List<ObraSocialUsuario> ObraSocialUsuarios { get; set; }
    }
}