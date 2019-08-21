using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Pago {

        [Key]
        public int IdPago { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int Monto { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered.")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        
        public int IdUsuario { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }

        
        public int? IdTarjeta { get; set; }

        [JsonIgnore]
        public virtual Tarjeta Tarjeta { get; set; }

        public int IdMedico { get; set; }

        [JsonIgnore]
        public virtual Medico Medico { get; set; }

    }
}
