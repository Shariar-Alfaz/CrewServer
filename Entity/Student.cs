using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Image { get; set; }
        public ICollection<StudentClassMap> Classes { get; set; }
    }
}
