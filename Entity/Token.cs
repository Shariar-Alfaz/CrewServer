using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Token
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public LoginInformation LoginInformation { get; set; }
    }
}
