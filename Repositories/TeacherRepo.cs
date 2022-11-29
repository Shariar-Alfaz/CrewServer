using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using InterFaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class TeacherRepo:ICommonRepository<Teacher>
    {
        private readonly DataContext Data;

        public TeacherRepo(DataContext data)
        {
            Data = data;
        }
        public async Task<List<Teacher>> GetAll()
        {
            return await Data.Teachers.ToListAsync();
        }

        public async Task<Teacher?> Save(Teacher entity)
        {
            var teacher = await Data.Teachers.AddAsync(entity);
            if (await this.Saved())
                return teacher.Entity;
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var teacher = await Data.Teachers.FirstOrDefaultAsync(f => f.Id == id);
            Data.Teachers.Remove(teacher);
            return await this.Saved();
        }

        public async Task<Teacher> Update(Teacher entity)
        {
            var teacher = await Task.Run(() => Data.Teachers.Update(entity));
            return teacher.Entity;
        }
        private async Task<bool> Saved()
        {
            var save = await Data.SaveChangesAsync();
            return save > 0 ? true : false;
        }

        public async Task<Teacher?> GetMe(string key)
        {
            var loginInfo = await Data.Tokens
                .Where(f=>f.Key.Equals(key))
                .Select(f=>f.LoginInformation)
                .FirstOrDefaultAsync();
            var teacher = await Data.Teachers
                .FirstOrDefaultAsync(f => f.Email.Equals(loginInfo.LoginEmail));
            return teacher;
        }
    }
}
