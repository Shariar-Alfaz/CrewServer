using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public async Task<SendData<Class?>> GetAllClasses(string key)
        {
            var sendData = new SendData<Class>()
            {
                Data = await TeacherRepo.GetAllClass(key)
            };
            return sendData;
        }

        public async Task<SendData<Class>> GetClass(int id)
        {
            var c = await TeacherRepo.GetClass(id);
            var sendData = new SendData<Class>()
            {
                SingleData = c,
                Success = c!=null,
                HasError = c==null,
                Message = c!=null?"":"Something Went wrong!",
            };
            return sendData;
        }

        public async Task<SendData<Student>> GetStudents( int id)
        {
            var students = await TeacherRepo.GetStudents(id);
            var send = new SendData<Student>()
            {
                Data = students
            };
            return send;
        }

        public async Task<SendData<StudentClassDTO>> SaveStudentsToClass(List<StudentClassDTO> students, int classId)
        {
            var complete = await TeacherRepo.SaveStudentsToClass(students, classId);
            var send = new SendData<StudentClassDTO>()
            {
                Data = complete
            };
            return send;
        }

        public async Task<SendData<Student>> GetClassStudents(int classId)
        {
            var data = new SendData<Student>()
            {
                Data = await TeacherRepo.GetClassStudents(classId)
            };
            return data;
        }

        public async Task<SendData<string>> RemoveStudentFromClass(int sId, int classId)
        {
            var check = await TeacherRepo.RemoveStudentFromClass(sId, classId);
            var send = new SendData<string>()
            {
                HasError = !check,
                Success = check,
                Message = check?"Removed from class.":"Something wrong"
            };
            return send;
        }
    }
}
