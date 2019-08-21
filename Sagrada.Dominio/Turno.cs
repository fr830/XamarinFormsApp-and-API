using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Turno {

        [Key]
        public int IdTurno { get; set; }

        public int IdMedico { get; set; }

        public int IdEspecialidad { get; set; }

        
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        public int? IdObraSocial { get; set; }

        [JsonIgnore]
        public virtual Especialidad Especialidad { get; set; }

        [JsonIgnore]
        public virtual ObraSocial ObraSocial { get; set; }

        [JsonIgnore]
        public virtual Medico Medico { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }
    }
}
