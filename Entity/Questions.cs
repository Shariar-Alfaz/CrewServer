using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Questions
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [ForeignKey("Exam")]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public ICollection<Options> Options { get; set; }
        public Answer Answer { get; set; }

    }
}
