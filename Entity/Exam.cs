using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public double TotalMarks { get; set; }
        public ICollection<Questions> Questions { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public ICollection<StudentExamMapping> Students { get; set; }
        public ICollection<Result> Results { get; set; }
        public ICollection<ExamBlock> ExamBlocks { get; set; }
    }
}
