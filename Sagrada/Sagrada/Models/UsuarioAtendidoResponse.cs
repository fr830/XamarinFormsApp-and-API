using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class UsuarioAtendidoResponse {


        public int IdUsuarioAtendido { get; set; }

        public int IdUsuario { get; set; }

        public int IdCobroMedico { get; set; }


        public string HistoriaClinica { get; set; }


        public DateTime Fecha { get; set; }

        public int? IdObraSocial { get; set; }

        public  ObraSocial ObraSocial { get; set; }


        public CobroMedico CobroMedico { get; set; }



        public Usuario Usuario { get; set; }
    }
}
