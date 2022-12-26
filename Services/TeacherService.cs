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

        public async Task<SendData<Exam>> SaveExam(ExamDTO exam)
        {
            var examNew = new Exam()
            {
                Title = exam.Title,
                ClassId = exam.ClassId,
                Description = exam.Description,
                EndTime = Convert.ToDateTime(exam.EndTime),
                StartTime = Convert.ToDateTime(exam.StartTime),
                TotalMarks = exam.TotalMarks,
            };
            var saved = await TeacherRepo.SaveExam(examNew);
            var send = new SendData<Exam>()
            {
                HasError = saved == null,
                Success = saved != null,
                Message = saved == null ? "This exam title id already exist." : "",
                SingleData = saved
            };
            return send;
        }

        public async Task<SendData<Exam>> GetExam(string key)
        {
            var send = new SendData<Exam>()
            {
                Data = await TeacherRepo.GetExams(key)
            };
            return send;
        }

        public async Task<SendData<string>> SaveQuestion(FinalQuestionDTO question)
        {
            var send = new SendData<string>();
            var qs = new Questions()
            {
                Title = question.Question.Title,
                ExamId = question.Question.ExamId,

            };
            var q = await TeacherRepo.SaveQuestion(qs);
            if (q == null)
            {
                send.HasError = true;
                send.Success = false;
                send.Message = "Question cannot be saved.";
                return send;
            }

            var ans = new Answer()
            {
                QuestionId = q.Id,
                answer = question.Question.Answer,
            };
            var a = await TeacherRepo.SaveAns(ans);
            if (!a)
            {
                send.HasError = true;
                send.Success = false;
                send.Message = "Answer cannot be saved.";
                return send;
            }

            List<Options> options = new List<Options>();
            Parallel.ForEach(question.Option, op =>
            {
                options.Add(this.ConvertOption(op,q.Id));
            });
            var o = await TeacherRepo.SaveOptions(options);
            if (!o)
            {
                send.HasError = true;
                send.Success = false;
                send.Message = "Options cannot be saved.";
                return send;
            }

            send.Message = "Successfully saved.";
            return send;
        }

        private Options ConvertOption(OptionDTO op,int questionId)
        {
            var options = new Options()
            {
                QuestionId = questionId,
                Name = op.Name,
            };
            return options;
        }

        public async Task<SendData<Questions>> GetQuestions(int examId)
        {
            var send = new SendData<Questions>()
            {
                Data = await TeacherRepo.GetQuestions(examId)
            };
            return send;
        }

        public async Task<SendData<ClassTask>> SaveClassTask(ClassTaskDTO classTaskDTO)
        {
            var classTask = new ClassTask()
            {
                Name = classTaskDTO.Name,
                Description = classTaskDTO.Description,
                ClassId = classTaskDTO.ClassId,
                TotalMarks = classTaskDTO.TotalMarks,
            };

            var data = await TeacherRepo.SaveTask(classTask);
            
            List<StudentClassTaskDetails> studentClassTaskDetails = new List<StudentClassTaskDetails>();

            if (data != null)
            {
                var students = await TeacherRepo.GetClassStudents(classTaskDTO.ClassId);
                Parallel.ForEach(students, student =>
                {
                    var std = new StudentClassTaskDetails()
                    {
                        ClassTaskId = data.Id,
                        StudentId = student.Id,
                    };
                    studentClassTaskDetails.Add(std);
                });

                await TeacherRepo.AssignStudentToTask(studentClassTaskDetails);

            }

            var send = new SendData<ClassTask>()
            {
                SingleData = data,
                HasError = data==null,
                Success = data!=null,
                Message = data==null? "Something went wrong": ""
            };
            return send;
        }

        public async Task<SendData<ClassTask>> GetClassTask(int classId)
        {
            var send = new SendData<ClassTask>()
            {
                Data = await TeacherRepo.GetClassTask(classId)
            };
            return send;
        }

        public async Task<SendData<Student>> GetStudentTaskDetails(int taskId)
        {
            var send = new SendData<Student>()
            {
                Data = await TeacherRepo.GetStudentTaskDetails(taskId)
            };
            return send;
        }
    }
}
