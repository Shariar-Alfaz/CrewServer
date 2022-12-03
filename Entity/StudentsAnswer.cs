using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class StudentsAnswer
    {
        public int Id { get; set; }
        [ForeignKey("Questions")]
        public int QuestionId { get; set; }
        public Questions Questions { get; set; }
        public string Answer { get; set; }
        [ForeignKey("AnswerScript")]
        public int AnswerScriptId { get; set; }
        public AnswerScript AnswerScript { get; set; }
    }
}
