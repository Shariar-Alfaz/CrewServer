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

    }
}
