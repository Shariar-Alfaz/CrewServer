using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TaskMonitor
    {
        public int Id { get; set; }
        [ForeignKey("StudentClassTaskDetails")]
        public int StudentClassDetailsId { get; set; }
        public StudentClassTaskDetails StudentClassTaskDetails { get; set; }
        [AllowNull]
        public int TotalKeypress { get; set; }
        public ICollection<TaskMonitorScreenShots> TaskMonitorScreenShotsCollection { get; set; }
    }
}
