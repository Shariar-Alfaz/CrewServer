using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public ICollection<StudentClassMap> Students { get; set; }
        public bool MakeArchive { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<ClassTask> ClassTasks { get; set; }
    }
}
