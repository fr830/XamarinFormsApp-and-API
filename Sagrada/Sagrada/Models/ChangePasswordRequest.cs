using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Models
{
    public class ChangePasswordRequest {

        public string CurrentPassword { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
