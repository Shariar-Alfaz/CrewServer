using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class StudentResDTO
    {
        [AllowNull]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public DateTime BirthDate { get; set; }
        [AllowNull]
        public string Password { get; set; }
        public string Date { get; set; }
    }
}
