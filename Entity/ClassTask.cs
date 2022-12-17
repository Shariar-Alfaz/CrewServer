using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ClassTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public double TotalMarks { get; set; }
        public ICollection<StudentClassTaskDetails> StudentClassTaskDetails { get; set; }
    }
}
