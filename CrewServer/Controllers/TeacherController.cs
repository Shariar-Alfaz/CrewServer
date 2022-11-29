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
        
    }
}
