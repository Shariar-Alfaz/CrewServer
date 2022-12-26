using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTO;
using Entity;
using Repositories;

namespace Services
{
    public class StudentService
    {
        private readonly StudentRepo StudentRepo;
        public StudentService(StudentRepo studentRepo)
        {
            StudentRepo = studentRepo;
        }
        public async Task<SendData<Student>> GetMe(string key)
        {
            var student = await StudentRepo.GetMe(key);
            var send = new SendData<Student>();
            if (student == null)
            {
                send.HasError = true;
                send.Message = "Something wrong!";
                send.Success = false;
            }
            else
            {
                send.SingleData = student;
            }
            return send;
        }
        public async Task<SendData<Class?>> GetAllClasses(string key)
        {
            var sendData = new SendData<Class>()
            {
                Data = await StudentRepo.GetAllClass(key)
            };
            return sendData;
        }
        public async Task<SendData<Class>> GetClass(int id)
        {
            var c = await StudentRepo.GetClass(id);
            var sendData = new SendData<Class>()
            {
                SingleData = c,
                Success = c != null,
                HasError = c == null,
                Message = c != null ? "" : "Something Went wrong!",
            };
            return sendData;
        }

        public async Task<SendData<Exam>> GetExams(int classId)
        {
            var send = new SendData<Exam>()
            {
                Data=await StudentRepo.GetExams(classId)
            };
            return send;
        }

        public async Task<SendData<Questions>> GetQuestions(int examId)
        {
            var ques = await StudentRepo.GetQuestion(examId);
            var sendData = new SendData<Questions>()
            {
                HasError = ques==null,
                Success = ques!=null,
                Data = ques,
                Message = ques==null?"Exam not started yet":""
            };
            return sendData;
        }

        public async Task<SendData<Exam>> GetExamById(int id,string appKey)
        {
            var exam = await StudentRepo.GetExamById(id, appKey);
            var sendData = new SendData<Exam>()
            {
                SingleData = exam,
                Message = exam==null?"You have been blocked from this exam.":"",
                HasError = exam==null,
                Success = exam!=null,
            };
            return sendData;    
        }

        public async Task<SendData<string>> BlockMe(int examId, string appKey)
        {
            var ExamBlock = new ExamBlock()
            {
                ExamId = examId,
            };
            var sendData = new SendData<string>()
            {
                Message = await StudentRepo.BlockMe(ExamBlock,appKey)?"Unethical activity detected.You have been blocked from this exam.":"",
                Success = true
            };
            return sendData;
        }

        public async Task<SendData<ClassTask>> GetClassTask(int classID)
        {
            var sendData = new SendData<ClassTask>()
            {
                Data = await StudentRepo.GetClassTask(classID)
            };
            return sendData;
        }
    }
}
