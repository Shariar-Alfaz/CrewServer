using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class FinalQuestionDTO
    {
        public QuestionDTO Question { get; set; }
        public List<OptionDTO> Option { get; set; } = new List<OptionDTO>();
    }
}
