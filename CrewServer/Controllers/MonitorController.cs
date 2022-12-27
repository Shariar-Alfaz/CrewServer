using System.Globalization;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class MonitorController : Controller
    {
        private readonly MonitorService MonitorService;
        private readonly IWebHostEnvironment _webHost;

        public MonitorController(MonitorService monitorService, IWebHostEnvironment webHost)
        {
            MonitorService = monitorService;
            _webHost = webHost;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var data = await MonitorService.Login(loginDTO);
            if ( data== null) return BadRequest();
            return Ok(data);
        }

        [HttpGet("Get/Classes/{id}")]
        public async Task<IActionResult> GetClasses(int id)
        {
            return Ok(await MonitorService.GetClasses(id));
        }

        [HttpPost("Save/Monitor")]
        public async Task<IActionResult> SaveMonitor( int studentId , [FromForm] IFormFile file)
        {
           
            string newFileName = file.FileName + DateTime.Now.ToString(CultureInfo.InvariantCulture)+studentId.ToString();
            if (!System.IO.Directory.Exists(@"wwwroot\Student\TaskMonitor\"+studentId.ToString()))
            {
                System.IO.Directory.CreateDirectory(@"wwwroot\Student\TaskMonitor\" + studentId);
            }
            return Ok();
        }
    }
}
