using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class ObraSocialUsuario {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdObraSocial { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int NroAfiliado { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }

        [JsonIgnore]
        public virtual ObraSocial ObraSocial { get; set; }
    }
}
