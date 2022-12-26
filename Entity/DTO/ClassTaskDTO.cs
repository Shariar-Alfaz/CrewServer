using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class ClassTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClassId { get; set; }
        public double TotalMarks { get; set; }
    }
}
