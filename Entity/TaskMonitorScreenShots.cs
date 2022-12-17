using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TaskMonitorScreenShots
    {
        public int Id { get; set; }
        public string ScreenShot { get; set; }
        [ForeignKey("TaskMonitor")]
        public int TaskMonitorId { get; set; }
        public TaskMonitor TaskMonitor { get; set; }

    }
}
