using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using InterFaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Repositories
{
    public class AdminRepo:ICommonRepository<Admin>
    {
        private readonly DataContext _dataContext;
        public AdminRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Admin?>> GetAll()
        {
            return await _dataContext.Admins.ToListAsync();
        }

        public async Task<Admin?> Save(Admin entity)
        {
            await _dataContext.AddAsync(entity);
            if (await this.Saved())
            {
                return await _dataContext.Admins
                    .FirstOrDefaultAsync(a => a.Email.Equals(entity.Email));
            }
            return null;
        }

        public async Task<bool> SaveLoginInfo(LoginInformation entity)
        {
            await _dataContext.AddAsync(entity);
            return await this.Saved();
        }
        public async Task<Admin?> GetAdmin(string email)
        {
            return await _dataContext.Admins.FirstOrDefaultAsync(a => a.Email.Equals(email));
        }
        public async Task<bool> CheckIfEmailExist(string email)
        {
            var admin = await _dataContext.LoginInformations
                .FirstOrDefaultAsync(a => a.LoginEmail.Equals(email));
            return admin != null ? true : false;
        }

        public async Task<bool> CheckIfUserIdExist(string id)
        {
            var admin = await _dataContext.LoginInformations
                .FirstOrDefaultAsync(a => a.LoginId.Equals(id));
            return admin != null ? true : false;
        }
        public async Task<bool> Delete(int id)
        {
            var delete = await _dataContext.Admins.FirstOrDefaultAsync(f=>f.Id==id);
            var login = await _dataContext.LoginInformations
                .FirstOrDefaultAsync(f => f.LoginEmail.Equals(delete.Email)); 
            _dataContext.Admins.Remove(delete);
            _dataContext.LoginInformations.Remove(login);
            return await this.Saved();
        }

        public async Task<Admin> Update(Admin entity)
        {
            await Task.Run(() =>
            {
                var admin = _dataContext.Admins.FirstOrDefault(f => f.Id == entity.Id);
                var loginInfo = _dataContext.LoginInformations
                    .FirstOrDefault(f => f.LoginEmail.Equals(admin.Email));
                loginInfo.LoginEmail = entity.Email;
                loginInfo.LoginId = entity.UserId;
                admin.Name = entity.Name;
                admin.Email = entity.Email;
                admin.UserId = entity.UserId;
                admin.BirthDate = entity.BirthDate;
                _dataContext.LoginInformations.Update(loginInfo);
            });
            await this.Saved();
            return entity;
        }

        private async Task<bool> Saved()
        {
            var save = await _dataContext.SaveChangesAsync();
            return save > 0 ? true : false;
        }

        public async Task<Admin> GetMe(string token)
        {
            var loginInfo = await  _dataContext.Tokens
                .Where(t => t.Key.Equals(token))
                .Select(t => t.LoginInformation).FirstOrDefaultAsync();
            var user = await _dataContext.Admins.FirstOrDefaultAsync(s =>s.Email.Equals(loginInfo.LoginEmail));
            return user;
        }

        public async Task<Teacher?> SaveTeacher(Teacher teacher)
        {
            await _dataContext.AddAsync(teacher);
            if (await this.Saved())
            {
                var t = await _dataContext.Teachers
                    .FirstOrDefaultAsync(f => f.Email.Equals(teacher.Email));
                return t;
            }

            return null;
        }
        public async Task<Teacher> UpdateTeacher(Teacher entity)
        {
            await Task.Run(() =>
            {
                var teacher = _dataContext.Teachers.FirstOrDefault(f => f.Id == entity.Id);
                var loginInfo = _dataContext.LoginInformations
                    .FirstOrDefault(f => f.LoginEmail.Equals(teacher.Email));
                loginInfo.LoginEmail = entity.Email;
                loginInfo.LoginId = entity.UseId;
                teacher.Name = entity.Name;
                teacher.Email = entity.Email;
                teacher.UseId = entity.UseId;
                teacher.BirthDate = entity.BirthDate;
                _dataContext.LoginInformations.Update(loginInfo);
            });
            await this.Saved();
            return entity;
        }
        public async Task<bool> DeleteTeacher(int id)
        {
            var delete = await _dataContext.Teachers.FirstOrDefaultAsync(f => f.Id == id);
            var login = await _dataContext.LoginInformations
                .FirstOrDefaultAsync(f => f.LoginEmail.Equals(delete.Email));
            _dataContext.Teachers.Remove(delete);
            _dataContext.LoginInformations.Remove(login);
            return await this.Saved();
        }

        public async Task<List<Teacher?>> GetAllTeacher()
        {
            return await _dataContext.Teachers.ToListAsync();
        }

        public async Task<Class?> SaveClass(Class newClass)
        {
            if (newClass.Id!=0)
            {
                var up = await Task.Run(() => _dataContext.Classes.Update(newClass));
                await this.Saved();
                return up.Entity;
            }
            var c= await _dataContext.Classes
                .FirstOrDefaultAsync(f=>f.Name.ToLower().Equals(newClass.Name.ToLower()));
            if (c != null) return null;
            var c1 = await _dataContext.Classes.AddAsync(newClass);
            if (await this.Saved()) return c1.Entity;
            return null;
        }

        public async Task<List<Class>> GetClasses()
        {
            return await _dataContext.Classes.ToListAsync();
        }

        public async Task<bool> DeleteClass(int id)
        {
            var classDelete = await _dataContext.Classes.FirstOrDefaultAsync(f => f.Id == id);
            if (classDelete != null) _dataContext.Classes.Remove(classDelete);
            return await this.Saved();
        }
    }
}
