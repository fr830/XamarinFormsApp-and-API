using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Especialidad {

        [Key]
        public int IdEspecialidad { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("Especialidad_Nombre_Index", IsUnique = true)]
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<CobroMedico> CobroMedicos { get; set; }
    }
}
