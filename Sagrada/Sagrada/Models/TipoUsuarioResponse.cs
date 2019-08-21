using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class TipoUsuarioResponse {


        public int IdTipoUsuario { get; set; }


        public string Nombre { get; set; }


        public List<Usuario> Usuarios { get; set; }
    }
}
