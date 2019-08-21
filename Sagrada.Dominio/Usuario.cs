using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagrada.Dominio {

    public class Usuario {

        [Key]
        public int IdUsuario { get; set; }

        
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Nombre { get; set; }

        
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [Index("User_DNI_Index", IsUnique = true)]
        public int DNI { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaDeNacimiento { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Display(Name = "Image")]
        public string PathImagen { get; set; }

        public int IdTipoUsuario { get; set; }

        [JsonIgnore]
        public virtual TipoUsuario TipoUsuario { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "Image")]
        public string FullPathImagen {

            get {
                if (string.IsNullOrEmpty(PathImagen)) {
                    return "noimage";
                }

                if (IdTipoUsuario == 3) {
                    return string.Format(
                        "http://sagradabackend.azurewebsites.net/{0}",
                        PathImagen.Substring(1));
                }

                return string.Format(
                        "http://sagradaapi.azurewebsites.net/{0}",
                        PathImagen.Substring(1));
            }
        }

        [Display(Name = "User")]
        public string FullName {

            get {
                return string.Format("{0} {1}", this.Nombre, this.Apellido);
            }
        }

        [JsonIgnore]
        public virtual ICollection<UsuarioEnEspera> UsuarioEnEsperas { get; set; }

        [JsonIgnore]
        public virtual ICollection<Turno> Turnos { get; set; }

        [JsonIgnore]
        public virtual ICollection<Pago> Pagos { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioAtendido> UsuarioAtendidos { get; set; }

        [JsonIgnore]
        public virtual ICollection<ObraSocialUsuario> ObraSocialUsuarios { get; set; }





        //[JsonIgnore]
        //public virtual ICollection<Board> Boards { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<Prediction> Predictions { get; set; }
    }
}
