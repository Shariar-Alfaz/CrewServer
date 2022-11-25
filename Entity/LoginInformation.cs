using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class LoginInformation
    {
        public int Id { get; set; }
        public string LoginEmail { get; set; }
        public string LoginId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt  { get; set; }
        public int RoleId { get; set;}
        public Role Role { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}
