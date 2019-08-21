using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Tarjeta {

        [Key]
        public int IdTarjeta { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered.")]
        [Index("Tarjeta_CodOperacion_Index", IsUnique = true)]
        public int CodOperacion { get; set; }

        public int IdBanco { get; set; }

        [JsonIgnore]
        public virtual ICollection<Pago> Pagos { get; set; }

        [JsonIgnore]
        public virtual Banco Banco { get; set; }
    }
}
