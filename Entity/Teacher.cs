using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UseId { get; set; }
        public string Image { get; set; }
        public DateTime BirthDate { get; set;}
        public ICollection<Class> Classes { get; set; }
    }
}
