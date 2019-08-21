using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class BancoResponse
    {
        public int IdBanco { get; set; }


        public string Nombre { get; set; }


        public virtual ICollection<Tarjeta> Tarjetas { get; set; }
    }
}
