using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services;
using Services.Tools;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly AllGuardService AllGuard;

        public LoginController(LoginService loginService,AllGuardService allGuard)
        {
            _loginService = loginService;
            AllGuard = allGuard;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await _loginService.VerifyLoginInfo(request);
            if (token == null) return StatusCode(422,"Not matched.");
            var refreshToken = await _loginService.GetRefreshToken(request);
            var saved = await _loginService.SaveToken(refreshToken);
            if (!saved) return StatusCode(405,"Fail for login");
            this.SetRefreshToken(refreshToken);
            var role = await _loginService.GetRole(request.Email);
            if (role == 1)
            {
                var admin = await AllGuard.GetAdmin(refreshToken.Key);
                return Ok(new
                {
                    key = token,
                    role = role,
                    GetMe = refreshToken.Key,
                    user=admin
                });
            }
            else if (role == 2)
            {
                var student=await AllGuard.GetStudent(refreshToken.Key);
                return Ok(new
                {
                    key = token,
                    role = role,
                    GetMe = refreshToken.Key,
                    user=student
                });
            }
            else if(role == 3)
            {
                var teacher = await AllGuard.GetTeacher(refreshToken.Key);
                return Ok(new
                {
                    key = token,
                    role = role,
                    GetMe = refreshToken.Key, 
                    user=teacher
                });
            }

            return StatusCode(405);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken)
        {
            var token = await AllGuard.GetToken(refreshToken.appKey);
            if (token == null) return Unauthorized();
            if (refreshToken.role == "admin")
            {
                var admin = await AllGuard.GetAdmin(token.Key);
                var gtoken = new TokenGenerator(admin.Name, "admin", admin.Email);
                return Ok(new
                {
                    key = gtoken.GetToken(),
                    role = "admin",
                    getMe = refreshToken
                });
            }else if (refreshToken.role == "student")
            {
                var student = await AllGuard.GetStudent(token.Key);
                var gt = new TokenGenerator(student.Name, "student", student.Email);
                return Ok(new
                {
                    key = gt.GetToken(),
                    role = "student",
                    getMe = refreshToken
                });
            }else if (refreshToken.role == "teacher")
            {
                var teacher = await AllGuard.GetTeacher(token.Key);
                var gt = new TokenGenerator(teacher.Name, "teacher", teacher.Email);
                return Ok(new
                {
                    key = gt.GetToken(),
                    role = "teacher",
                    getMe = refreshToken
                });
            }

            return Unauthorized();
        }

       
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout( [FromHeader] string appKey)
        {
            if (appKey == null) return BadRequest();
            var entityToken = await AllGuard.GetToken(appKey);
            bool check = await Task.Run(() => AllGuard.UpdateToken(entityToken));
            return check ? Ok() : BadRequest();
        }

        private void SetRefreshToken(Token token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = token.ExpiredAt,
                Secure=true
            };
            Response.Cookies.Append("RefreshToken", token.Key, cookieOptions);
        }
    }
}
