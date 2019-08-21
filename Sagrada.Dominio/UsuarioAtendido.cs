using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class UsuarioAtendido {

        [Key]
        public int IdUsuarioAtendido { get; set; }

        public int IdUsuario { get; set; }
        
        public int IdCobroMedico { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(200, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string HistoriaClinica { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        public int? IdObraSocial { get; set; }

        [JsonIgnore]
        public virtual ObraSocial ObraSocial { get; set; }

        [JsonIgnore]
        public virtual CobroMedico CobroMedico { get; set; }

      
        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }
    }
}
