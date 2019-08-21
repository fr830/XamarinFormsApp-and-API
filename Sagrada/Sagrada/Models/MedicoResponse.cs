using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class MedicoResponse {


        public int IdMedico { get; set; }



        public string Nombre { get; set; }



        public string Apellido { get; set; }


        public int DNI { get; set; }

        public DateTime FechaDeNacimiento { get; set; }

        public string Email { get; set; }


        public string Telefono { get; set; }


        public string PathImagen { get; set; }


        public string FullPathImagen {

            get {
                if (string.IsNullOrEmpty(PathImagen)) {
                    return "noimage";
                }

                return string.Format(
                    "http://landsapi1.azurewebsites.net/{0}",
                    PathImagen.Substring(1));
            }
        }


        public string FullName {

            get {
                return string.Format("{0} {1}", this.Nombre, this.Apellido);
            }
        }

        public List<UsuarioEnEspera> UsuarioEnEsperas { get; set; }


        public List<Turno> Turnos { get; set; }

        public List<ObraSocialMedico> ObraSocialMedicos { get; set; }

        public List<Horario> Horarios { get; set; }


        public List<CobroMedico> CobroMedicos { get; set; }

        
        public  List<Pago> Pagos { get; set; }
    }
}
