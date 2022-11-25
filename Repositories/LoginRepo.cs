using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class LoginRepo
    {
        private readonly DataContext _dataContext;

        public LoginRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<LoginInformation> GetLoginInfo(LoginDTO loginDto)
        {
            var info = await _dataContext
                    .LoginInformations
                    .FirstOrDefaultAsync(li => li.LoginEmail
                        .Equals(loginDto.Email)
                    ||li.LoginId.Equals(loginDto.Email));
            return info;
        }

        public async Task<Student> GetStudent(string email)
        {
            var student = await _dataContext
                .Students
                .FirstOrDefaultAsync(s=>s.Email.Equals(email)
                || s.UserId.Equals(email));
            return student;
        }
        public async Task<Teacher> GetTeacher(string email)
        {
            var teacher = await _dataContext
                .Teachers
                .FirstOrDefaultAsync(s => s.Email.Equals(email)
                || s.UseId.Equals(email));
            return teacher;
        }
        public async Task<Admin> GetAdmin(string email)
        {
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(a => a.Email.Equals(email) ||
                                                                           a.UserId.Equals(email));
            return admin;
        }

        public async Task<int> GetRole(string email)
        {
            var role = await _dataContext
                .LoginInformations
                .Where(r => r.LoginEmail.Equals(email) ||
                            r.LoginId.Equals(email)).Select(r => r.RoleId)
                .FirstOrDefaultAsync();
            return role;

        }

        public async Task<Token> GetRefreshToken(LoginDTO loginDto)
        {
            var loginIfo = await this.GetLoginInfo(loginDto);
            var token = new Token()
            {
                Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(7),
                LoginInformation = loginIfo
            };
            return token;
        }

        public async Task<bool> SaveToken(Token tk)
        {
            await Task.Run(() => _dataContext.Add(tk));
            return Saved();
        }

        private bool Saved()
        {
            var save = _dataContext.SaveChanges();
            return save > 0 ? true : false;
        }
    }
}
