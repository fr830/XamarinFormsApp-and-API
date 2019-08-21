using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class ObraSocialMedico {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdObraSocial { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMedico { get; set; }

        [JsonIgnore]
        public virtual Medico Medico { get; set; }

        [JsonIgnore]
        public virtual ObraSocial ObraSocial { get; set; }
    }
}
