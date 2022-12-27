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

        [HttpGet("Get/ClassTask/{id}")]
        public async Task<IActionResult> GetClassTask(int id)
        {
            return Ok(await MonitorService.GetClassTask(id));
        }

        [HttpPost("Save/Monitor")]
        public async Task<IActionResult> SaveMonitor( int studentId )
        {
            
            return Ok();
        }
    }
}
