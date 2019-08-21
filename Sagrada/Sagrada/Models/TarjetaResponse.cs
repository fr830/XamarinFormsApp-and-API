using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class TarjetaResponse {


        public int IdTarjeta { get; set; }



        public int CodOperacion { get; set; }

        public int IdBanco { get; set; }


        public virtual ICollection<Pago> Pagos { get; set; }


        public virtual Banco Banco { get; set; }
    }
}
