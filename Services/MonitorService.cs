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

    }
}
