using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class TokenRepo
    {
        private readonly DataContext Data;
        public TokenRepo(DataContext data)
        {
            Data = data;
        }

        public async Task<Admin?> GetAdmin(string token)
        {
            var loginInfo = await Data.Tokens
                .Where(tk => tk.Key.Equals(token))
                .Select(tk => tk.LoginInformation).FirstOrDefaultAsync();
            var admin = await Data.Admins
                .FirstOrDefaultAsync(a => a.Email.Equals(loginInfo.LoginEmail));
            return admin;
        }
        public async Task<Teacher?> GetTeacher(string token)
        {
            var loginInfo = await Data.Tokens
                .Where(tk => tk.Key.Equals(token))
                .Select(tk => tk.LoginInformation).FirstOrDefaultAsync();
            var teacher = await Data.Teachers
                .FirstOrDefaultAsync(a => a.Email.Equals(loginInfo.LoginEmail));
            return teacher;
        }
        public async Task<Student?> GetStudent(string token)
        {
            var loginInfo = await Data.Tokens
                .Where(tk => tk.Key.Equals(token))
                .Select(tk => tk.LoginInformation).FirstOrDefaultAsync();
            var student = await Data.Students
                .FirstOrDefaultAsync(a => a.Email.Equals(loginInfo.LoginEmail));
            return student;
        }
    }
}
