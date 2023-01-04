using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using Entity.DTO;
using InterFaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class MonitorRepo
    {
        private readonly DataContext DataContext;

        public MonitorRepo(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<LoginInformation?> GetLoginInfo(LoginDTO info)
        {
            var loginInfo = await DataContext
                .LoginInformations
                .FirstOrDefaultAsync(f=>f.LoginEmail.Equals(info.Email) ||
                                        f.LoginId.Equals(info.Email));
            return loginInfo;
        }
        public async Task<List<ClassTask>> GetClassTask(int studentId)
        {
            var classes = await DataContext.StudentClassTaskDetails
                .Where(f => f.StudentId == studentId)
                .Select(f => f.ClassTask).ToListAsync();
            return classes;
        }
        public async Task<Student?> GetStudent(string email)
        {
            var student = await DataContext
                .Students
                .FirstOrDefaultAsync(s => s.Email.Equals(email)
                                          || s.UserId.Equals(email));
            return student;
        }
        public async Task<int> GetDetails(int studenId,int taskId)
        {
            return await DataContext.StudentClassTaskDetails
                .Where(f => f.ClassTaskId == taskId && f.StudentId == studenId)
                .Select(f => f.Id).FirstOrDefaultAsync();
                 
        }
        public async Task<TaskMonitor?> SaveTaskMonitor(TaskMonitor monitor)
        {
           if(monitor.Id == 0)
            {
                var m = await DataContext.TaskMonitors.AddAsync(monitor);
                if (await this.Saved())
                    return m.Entity;
            }
            var n = DataContext.TaskMonitors.Update(monitor);
            if (await this.Saved())
            {
                return n.Entity;
            }
            return null;
        }

        public async Task<TaskMonitor?> CheckForExistMonitor(int studentId , int taskId)
        {
            var monitor = await DataContext.StudentClassTaskDetails.Where(f => f.ClassTaskId == taskId && f.StudentId == studentId)
                .Select(f => f.TaskMonitor).FirstOrDefaultAsync();
            return monitor;
        }
        public async Task<bool> SaveScreenShots(TaskMonitorScreenShots screenShots)
        {
            await DataContext.TaskMonitorScreenShots.AddAsync(screenShots);
            return await this.Saved();
        }
        private async Task<bool> Saved()
        {
            var save =  await DataContext.SaveChangesAsync();
            return save > 0;
        }
    }
}
