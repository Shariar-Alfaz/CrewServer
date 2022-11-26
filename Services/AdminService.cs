using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTO;
using Microsoft.Identity.Client;
using Repositories;
using Services.Tools;

namespace Services
{
    public class AdminService
    {
        private readonly AdminRepo _adminRepo;
        public AdminService(AdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<Admin> SaveAdmin(AdminResDTO admin)
        {
            var newAdmin = this.ConvertAdmin(admin);
            var data= await _adminRepo.Save(newAdmin);
            if (data != null)
            {
                var hash = new Hash(admin.Password);
                var loginInfo = new LoginInformation()
                {
                    LoginEmail = data.Email,
                    LoginId = data.UserId,
                    PasswordHash = hash.GetHash(),
                    PasswordSalt = hash.GetSalt(),
                    RoleId = 1
                };
                if (await _adminRepo.SaveLoginInfo(loginInfo)) return data;
                return null;
            }

            return data;
        }

        private  Admin ConvertAdmin(AdminResDTO admin)
        {
            var hash = new Hash(admin.Password);

            var data =new Admin()
            {
                Email = admin.Email,
                Name = admin.Name,
                UserId = admin.UserId,
                BirthDate = Convert.ToDateTime(admin.Date),
            };
            return data;
        }

        public async Task<bool> CheckIfEmailExist(string email)
        {
            return await _adminRepo.CheckIfEmailExist(email);
        }

        public async Task<bool> CheckIfUserIdExist(string id)
        {
            return await _adminRepo.CheckIfUserIdExist(id);
        }

        public async Task<Admin> GetMe(string token)
        {
            return  await _adminRepo.GetMe(token);
        }

        public async Task<List<Admin?>> GetAll()
        {
            return await _adminRepo.GetAll();
        }

        public async Task<SendData<Admin>> UpdateAdmin(AdminResDTO admin)
        {
            var up = new Admin()
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                BirthDate = Convert.ToDateTime(admin.Date),
                UserId = admin.UserId
            };
            var res = await _adminRepo.Update(up);
            var data = new SendData<Admin>()
            {
                HasError = res == null,
                Message = res == null ? "Not saved" : "",
                Success = res != null,
                SingleData = res
            };
            return data;
        }

        public async Task<bool> DeleteAdmin(int id)
        {
            return  await _adminRepo.Delete(id);
        }

        public async Task<Teacher?> SaveTeacher(TeacherResDTO teacher)
        {
            var t = new Teacher()
            {
                Email = teacher.Email,
                UseId = teacher.UseId,
                Name = teacher.Name,
                BirthDate = Convert.ToDateTime(teacher.Date),
                Image="N/A"
            };
            var pass = new Hash(teacher.Password);
            var login = new LoginInformation()
            {
                LoginEmail = teacher.Email,
                LoginId = teacher.UseId,
                PasswordHash = pass.GetHash(),
                PasswordSalt = pass.GetSalt(),
                RoleId = 3
            };
            var data = await _adminRepo.SaveTeacher(t);
            var log = await _adminRepo.SaveLoginInfo(login);
            if (data != null && log) return data;
            return null;
        }
        public async Task<SendData<Teacher>> UpdateTeacher(TeacherResDTO teacher)
        {
            var up = new Teacher()
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                BirthDate = Convert.ToDateTime(teacher.Date),
                UseId = teacher.UseId,
            };
            var res = await _adminRepo.UpdateTeacher(up);
            var data = new SendData<Teacher>()
            {
                HasError = res == null,
                Message = res == null ? "Not saved" : "",
                Success = res != null,
                SingleData = res
            };
            return data;
        }
        public async Task<bool> DeleteTeacher(int id)
        {
            return await _adminRepo.DeleteTeacher(id);
        }

        public async Task<List<Teacher?>> GetAllTeacher()
        {
            return await _adminRepo.GetAllTeacher();
        }
    }
}
