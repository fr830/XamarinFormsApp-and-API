using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class CobroMedico {

        [Key]
        public int IdCobroMedico { get; set; }

        
        public int IdMedico { get; set; }

        
        public int IdEspecialidad { get; set; }

      

        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int Honorarios { get; set; }

        [JsonIgnore]
        public virtual Medico Medico { get; set; }

        [JsonIgnore]
        public virtual Especialidad Especialidad { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioAtendido> UsuarioAtendidos { get; set; }

    }
}
