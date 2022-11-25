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
            var admin = await _dataContext.Admins
                .FirstOrDefaultAsync(a => a.Email.Equals(email));
            return admin != null ? true : false;
        }

        public async Task<bool> CheckIfUserIdExist(string id)
        {
            var admin = await _dataContext.Admins.FirstOrDefaultAsync(a => a.UserId.Equals(id));
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

        public async Task<Admin> Update(Admin? entity)
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

    }
}
