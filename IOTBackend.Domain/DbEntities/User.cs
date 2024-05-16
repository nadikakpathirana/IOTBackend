using IOTBackend.Domain.DbEntities.BaseEntities;
using IOTBackend.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Domain.DbEntities
{
    public class User : ModelBase
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Username {  get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; }

        public UserType UserType { get; set; }

        [NotMapped]
        public string Token { get; set; } = string.Empty;
    }
}
