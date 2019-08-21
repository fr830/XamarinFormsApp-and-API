using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Horario {

        [Key]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Dia { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]       
        public int HoraInicio { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int HoraFin { get; set; }       

        public int IdMedico { get; set; }

        [JsonIgnore]
        public virtual Medico Medico { get; set; }
    }
}
