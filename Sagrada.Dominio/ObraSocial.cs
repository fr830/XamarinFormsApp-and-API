using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class ObraSocial {

        [Key]
        public int IdObraSocial { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<ObraSocialUsuario> ObraSocialUsuarios { get; set; }

        [JsonIgnore]
        public virtual ICollection<ObraSocialMedico> ObraSocialMedicos { get; set; }

        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioEnEspera> UsuarioEnEsperas { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioAtendido> UsuarioAtendidos { get; set; }
    }
}
