using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationOutage.ViewModels
{
    public class LoginModel
    {
       
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string EncryptPassword{ get; set; }
    }
}