using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTO;
using Repositories;
using Services.Tools;

namespace Services
{
    public class LoginService
    {
        private readonly LoginRepo _loginRepo;

        public LoginService(LoginRepo loginRepo)
        {
            _loginRepo = loginRepo;
        }

        public async Task<string> VerifyLoginInfo(LoginDTO loginDTO)
        {
            var info = await _loginRepo.GetLoginInfo(loginDTO);
            if (info != null)
            {
                var mached = await Task.Run(() => Hash
                    .VerifyPassword(loginDTO.Password, info.PasswordHash, info.PasswordSalt));
                if (mached)
                {
                    var role = await _loginRepo.GetRole(loginDTO.Email);
                    if (role > 0)
                    {
                        if (role==1)
                        {
                            var admin = await _loginRepo.GetAdmin(loginDTO.Email);
                            var token = new TokenGenerator(admin.Name, "admin", admin.Email);
                            return token.GetToken();
                        }
                        else if (role==2)
                        {
                            var student = await _loginRepo.GetStudent(loginDTO.Email);
                            var token = new TokenGenerator(student.Name, "student", student.Email);
                            return token.GetToken();
                        }
                        else if(role==3)
                        {
                            var teacher = await _loginRepo.GetTeacher(loginDTO.Email);
                            var token = new TokenGenerator(teacher.Name,"teacher", teacher.Email);
                            return token.GetToken();
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Token> GetRefreshToken(LoginDTO loginDto)
        {
            return await _loginRepo.GetRefreshToken(loginDto);
        }

        public async Task<bool> SaveToken(Token tk)
        {
            return await _loginRepo.SaveToken(tk);
        }

        public async Task<int> GetRole(string emailOrUserId)
        {
           
            return await _loginRepo.GetRole(emailOrUserId);
        }
    }
}
