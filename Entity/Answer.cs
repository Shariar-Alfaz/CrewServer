using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Answer
    {
        public int Id { get; set; }
        public string answer { get; set; }
        [ForeignKey("Questions")]
        public int QuestionId { get; set; }
        public Questions Questions { get; set; }
    }
}
