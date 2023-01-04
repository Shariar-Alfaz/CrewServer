using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    //[Authorize(Roles = "teacher")]
    public class TeacherController : Controller
    {
        private readonly TeacherService TeacherService;

        public TeacherController(TeacherService teacherService)
        {
            TeacherService = teacherService;
        }
        [HttpGet("GetMe")]
        public async Task<IActionResult> GetMe([FromHeader] string appkey)
        {
            if (appkey == null) return StatusCode(401);
            return Ok(await TeacherService.GetMe(appkey));
        }

        [HttpGet("Get/Classes")]
        public async Task<IActionResult> GetAllClasses([FromHeader] string appkey)
        {
            if (appkey == null) return StatusCode(401);
            var data = await TeacherService.GetAllClasses(appkey);
            return Ok(data);
        }

        [HttpGet("Get/Class/{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await TeacherService.GetClass(id));
        } 

        [HttpGet("Get/Students/{id}")]
        public async Task<IActionResult> GetStudents(int id)
        {
            return Ok(await TeacherService.GetStudents(id));
        }

        [HttpPost("Save/Students/ToClass")]
        public async Task<IActionResult> SaveStudentsToClass([FromBody] List<StudentClassDTO> students,
            [FromHeader] int classId)
        {
            return Ok(await TeacherService.SaveStudentsToClass(students, classId));
        }

        [HttpGet("Get/Students/Class/{classId}")]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            return Ok(await TeacherService.GetClassStudents(classId));
        }

        [HttpDelete("Delete/Student/Class/{sId}/{classId}")]
        public async Task<IActionResult> RemoveStudentFromClass(int sId, int classId)
        {
            return Ok(await TeacherService.RemoveStudentFromClass(sId, classId));
        }

        [HttpPost("Save/Exam")]
        public async Task<IActionResult> SaveExam([FromBody] ExamDTO exam)
        {
            return Ok(await TeacherService.SaveExam(exam));
        }

        [HttpGet("Get/Exam")]
        public async Task<IActionResult> GetExam([FromHeader] string appkey)
        {
            return Ok(await TeacherService.GetExam(appkey));
        }

        [HttpPost("Save/Question")]
        public async Task<IActionResult> SaveQuestion([FromBody] FinalQuestionDTO question)
        {
            return Ok(await TeacherService.SaveQuestion(question));
        }

        [HttpGet("Get/Questions/{examId}")]
        public async Task<IActionResult> GetQuestions(int examId)
        {
            return Ok(await TeacherService.GetQuestions(examId));
        }

        [HttpPost("Save/Task")]
        public async Task<IActionResult> SaveTask(ClassTaskDTO classTaskDto)
        {
            return Ok(await TeacherService.SaveClassTask(classTaskDto));
        }

        [HttpGet("Get/ClassTask/{id}")]
        public async Task<IActionResult> GetClassTask(int id)
        {
            return Ok(await TeacherService.GetClassTask(id));
        }

        [HttpGet("Get/GetStudentTaskDetails/{id}")]
        public async Task<IActionResult> GetStudentTaskDetails(int id)
        {
            return Ok(await TeacherService.GetStudentTaskDetails(id));
        }
        [HttpGet("Get/Student/Task/Monitor/{studentId}/{taskId}")]
        public async Task<IActionResult> GetStudentTaskMonitor(int studentId,int taskId)
        {
            return Ok(await TeacherService.GetStudentMonitor(studentId, taskId));
        }
    }
}
