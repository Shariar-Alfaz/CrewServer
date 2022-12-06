using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        public int ExamId  { get; set; }
    }
}
