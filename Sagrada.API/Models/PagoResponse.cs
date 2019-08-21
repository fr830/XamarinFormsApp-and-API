﻿using Sagrada.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sagrada.API.Models {

    public class PagoResponse {

        
        public int IdPago { get; set; }


        
        public int Monto { get; set; }


        
        public DateTime Fecha { get; set; }


        public int IdUsuario { get; set; }

        
        public  Usuario Usuario { get; set; }


        public int? IdTarjeta { get; set; }

        
        public  Tarjeta Tarjeta { get; set; }

        public int IdMedico { get; set; }

        
        public  Medico Medico { get; set; }
    }
}