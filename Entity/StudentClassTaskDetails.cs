using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        [AllowNull]
        public double ObtainMarks { get; set; }

        //TODO need to change
        [AllowNull]
        public string SubmitedFile { get; set; }
        [AllowNull]
        public TaskMonitor TaskMonitor { get; set; }
    }
}
