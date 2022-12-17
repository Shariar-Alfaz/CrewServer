using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class StudentClassTaskDetails
    {
        public int Id { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey("ClassTask")] 
        public int ClassTaskId { get; set; }
        public ClassTask ClassTask { get; set; }
        public double ObtainMarks { get; set; }
        public string SubmitedFile { get; set; }
        public TaskMonitor TaskMonitor { get; set; }
    }
}
