
using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CrewServer.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly AllGuardService _allGuard;

        public AdminController(AdminService adminService, AllGuardService allGuard)
        {
            _adminService = adminService;
            _allGuard = allGuard;
        }

        #region Admin

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] AdminResDTO request)
        {
            var isExistEmail = await _adminService.CheckIfEmailExist(request.Email);
            var isUserIdExist = await _adminService.CheckIfUserIdExist(request.UserId);
            if (isExistEmail || isUserIdExist) return StatusCode(423, "Account already exist");
            var admin = await _adminService.SaveAdmin(request);
            if (admin == null) return StatusCode(420, "Internal Server problem try again letter.");
            var sendData = new SendData<Admin>();
            sendData.SingleData = admin;
            sendData.Message = "Successfully saved.";
            sendData.HasError = false;
            sendData.Success = true;
            return Ok(sendData);
        }

        [HttpGet("GetMe")]
        public async Task<IActionResult> GetMe([FromHeader]string appkey)
        {
            var check = await this.Check(appkey);
            if (check==null) return StatusCode(403);
            var admin = await _adminService.GetMe(appkey);
            if (admin == null) return BadRequest();
            SendData<Admin> sendData = new SendData<Admin>();
            sendData.SingleData = admin;
            sendData.HasError=false;
            sendData.Success = true;
            return Ok(sendData);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var admins = await _adminService.GetAll();
            var sendData = new SendData<Admin>
            {
                HasError = false,
                Success = true,
                Data = admins
            };
            return Ok(sendData);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] AdminResDTO admin)
        {
            return Ok(await _adminService.UpdateAdmin(admin));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (await _adminService.DeleteAdmin(id))
            {
                var sendDate = new SendData<string>()
                {
                    HasError = false,
                    Success = true,
                };
                return Ok(sendDate);
            }
            return BadRequest();
        }
        private async Task<Token?> Check(string app)
        {
            var token = await _allGuard.GetToken(app);
            return token;
        }


        #endregion

        #region Teacher

        [HttpPost("Teacher/Save")]
        public async Task<IActionResult> SaveTeacher([FromBody] TeacherResDTO teacher)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var isUserEmailExist = await _adminService.CheckIfEmailExist(teacher.Email);
            var isUserIdExist = await _adminService.CheckIfUserIdExist(teacher.UseId);
            if (isUserEmailExist || isUserIdExist) return StatusCode(423, "Email or user id is already exist");
            var t = await _adminService.SaveTeacher(teacher);
            var sendData = new SendData<Teacher>()
            {
                HasError = false,
                Success = true,
                SingleData = t
            };
            return Ok(sendData);
        }

       
        [HttpDelete("Delete/Teacher/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            if (await _adminService.DeleteTeacher(id))
            {
                var sendDate = new SendData<string>()
                {
                    HasError = false,
                    Success = true,
                };
                return Ok(sendDate);
            }
            return BadRequest();
        }

        [HttpPost("Update/Teacher")]
        public async Task<IActionResult> UpdateTeacher([FromBody] TeacherResDTO teacher)
        {
            return Ok(await _adminService.UpdateTeacher(teacher));
        }

        [HttpGet("Get/Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _adminService.GetAllTeacher();
            var sendData = new SendData<Teacher>
            {
                HasError = false,
                Success = true,
                Data = teachers
            };
            return Ok(sendData);
        }

        #endregion

        #region Student

        [HttpPost("Student/Save")]
        public async Task<IActionResult> SaveStudent([FromBody] StudentResDTO student)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var isUserEmailExist = await _adminService.CheckIfEmailExist(student.Email);
            var isUserIdExist = await _adminService.CheckIfUserIdExist(student.UserId);
            if (isUserEmailExist || isUserIdExist) return StatusCode(423, "Email or user id is already exist");
            var t = await _adminService.SaveStudent(student);
            var sendData = new SendData<Student>()
            {
                HasError = false,
                Success = true,
                SingleData = t
            };
            return Ok(sendData);
        }


        [HttpDelete("Delete/Student/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (await _adminService.DeleteStudent(id))
            {
                var sendDate = new SendData<string>()
                {
                    HasError = false,
                    Success = true,
                };
                return Ok(sendDate);
            }
            return BadRequest();
        }

        [HttpPost("Update/Student")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentResDTO student)
        {
            return Ok(await _adminService.UpdateStudent(student));
        }

        [HttpGet("Get/Student")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _adminService.GetAllStudent();
            var sendData = new SendData<Student>
            {
                HasError = false,
                Success = true,
                Data = students
            };
            return Ok(sendData);
        }

        #endregion

        #region Class

        [HttpGet("Get/Classes")]
        public async Task<IActionResult> GetClasses()
        {
            return Ok(await _adminService.GetClasses());
        }

        [HttpPost("Save/Class")]
        public async Task<IActionResult> SaveClass([FromBody] ClassDTO request)
        {
           // if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _adminService.SaveClass(request));
        }

        [HttpDelete("Delete/Class/{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            return Ok(await _adminService.DeleteClass(id));
        }

        #endregion
    }
}
