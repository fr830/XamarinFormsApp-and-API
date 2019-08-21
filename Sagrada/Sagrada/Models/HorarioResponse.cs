using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class HorarioResponse {


        public int IdHorario { get; set; }


        public string Dia { get; set; }


        public int HoraInicio { get; set; }


        public int HoraFin { get; set; }

        public int IdMedico { get; set; }


        public Medico Medico { get; set; }
    }
}
