using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Banco {

        [Key]
        public int IdBanco { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tarjeta> Tarjetas { get; set; }
    }
}
