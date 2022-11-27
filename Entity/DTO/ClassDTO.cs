using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class ClassDTO
    {
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public bool MakeArchive { get; set; }
        [AllowNull]
        public int Id { get; set; }
    }
}
