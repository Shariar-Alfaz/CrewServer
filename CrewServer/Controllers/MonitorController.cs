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

        public MonitorController(MonitorService monitorService)
        {
            MonitorService = monitorService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            return Ok();
        }

        [HttpGet("Get/Classes/{id}")]
        public async Task<IActionResult> GetClasses(int id)
        {
            return Ok(await MonitorService.GetClasses(id));
        }
      
    }
}
