using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    [Authorize(Roles = "teacher")]
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

    }
}
