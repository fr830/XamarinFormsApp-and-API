using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class BancoResponse {

       
        public int IdBanco { get; set; }

        
        public string Nombre { get; set; }

        
        public virtual ICollection<Tarjeta> Tarjetas { get; set; }
    }
}