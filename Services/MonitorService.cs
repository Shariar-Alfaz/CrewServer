using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTO;
using Repositories;
using Services.Tools;

namespace Services
{
    public class MonitorService
    {
        private readonly MonitorRepo Monitor;

        public MonitorService(MonitorRepo monitor)
        {
            Monitor = monitor;
        }

        public async Task<Student?> Login(LoginDTO loginDto)
        {
            var loginInfo = await Monitor.GetLoginInfo(loginDto);
            if (loginInfo == null) return null;
            if(!Hash.VerifyPassword(loginDto.Password,loginInfo.PasswordHash,loginInfo.PasswordSalt)) return null;
            return await Monitor.GetStudent(loginDto.Email);
        }
        public async Task<List<ClassTask>> GetClassTask(int studentId)
        {
            return await Monitor.GetClassTask(studentId);
        }
        public async Task SaveMonitor(int studentId,int taskId,int keyPress, string path)
        {
            var details = await Monitor.GetDetails(studentId, taskId);
            var monitor = await Monitor.CheckForExistMonitor(studentId,taskId);
            if (monitor == null)
            {
                var newMonitor = new TaskMonitor()
                {
                    StudentClassDetailsId = details,
                    TotalKeypress= keyPress,
                };
                var save = await Monitor.SaveTaskMonitor(newMonitor);
                if(save != null)
                {
                    var screenShot = new TaskMonitorScreenShots()
                    {
                        TaskMonitorId = save.Id,
                        DateTime = DateTime.Now,
                        ScreenShot = path,
                    };
                    await Monitor.SaveScreenShots(screenShot);
                }
            }
            else
            {
                monitor.TotalKeypress += keyPress;
                var save = await Monitor.SaveTaskMonitor(monitor);
                if (save != null)
                {
                    var screenShot = new TaskMonitorScreenShots()
                    {
                        TaskMonitorId = save.Id,
                        DateTime = DateTime.Now,
                        ScreenShot = path,
                    };
                    await Monitor.SaveScreenShots(screenShot);
                }
            }
        }
    }
}
