using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        private readonly StudentService StudentService;
        public StudentController(StudentService studentService)
        {
            StudentService = studentService;
        }
        [HttpGet("GetMe")]
        public async Task<IActionResult> GetMe([FromHeader] string appkey)
        {
            if (appkey == null) return StatusCode(401);
            return Ok(await StudentService.GetMe(appkey));
        }
        [HttpGet("Get/Classes")]
        public async Task<IActionResult> GetAllClasses([FromHeader] string appkey)
        {
            if (appkey == null) return StatusCode(401);
            var data = await StudentService.GetAllClasses(appkey);
            return Ok(data);
        }

        [HttpGet("Get/Class/{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await StudentService.GetClass(id));
        }

        [HttpGet("Get/Exams/{id}")]
        public async Task<IActionResult> GetExams(int id)
        {
            return Ok(await StudentService.GetExams(id));
        }

        [HttpGet("Get/Questions/{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            return Ok(await StudentService.GetQuestions(id));
        }

        [HttpGet("Get/ExamById/{id}")]
        public async Task<IActionResult> GetExamById(int id, [FromHeader] string appkey)
        {
            return Ok(await StudentService.GetExamById(id,appkey));
        }

        [HttpGet("Block/Me/{id}")]
        public async Task<IActionResult> BlockMe(int id, [FromHeader] string appkey)
        {
            return Ok(await StudentService.BlockMe(id, appkey));
        }
    }
}
