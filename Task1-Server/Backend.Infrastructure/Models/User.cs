using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<FillingGroup> FillingGroups { get; set; }
    }
}
