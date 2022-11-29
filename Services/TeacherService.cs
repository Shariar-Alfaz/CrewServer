using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTO;
using Repositories;

namespace Services
{
    public class TeacherService
    {
        private readonly TeacherRepo TeacherRepo;

        public TeacherService(TeacherRepo teacherRepo)
        {
            TeacherRepo = teacherRepo;
        }

        public async Task<SendData<Teacher>> GetMe(string key)
        {
            var teacher = await TeacherRepo.GetMe(key);
            var send = new SendData<Teacher>();
            if (teacher == null)
            {
                send.HasError = true;
                send.Message = "Something wrong!";
                send.Success = false;
            }
            else
            {
                send.SingleData = teacher;
            }
            return send;
        }
    }
}
