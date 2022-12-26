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
    public class StudentRepo:ICommonRepository<Student>
    {
        private readonly DataContext DataContext;
        public StudentRepo(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        public Task<List<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Student?> Save(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Update(Student entity)
        {
            throw new NotImplementedException();
        }
        public async Task<Student?> GetMe(string key)
        {
            var loginInfo = await DataContext.Tokens
                .Where(f => f.Key.Equals(key))
                .Select(f => f.LoginInformation)
                .FirstOrDefaultAsync();
            var student = await DataContext.Students
                .FirstOrDefaultAsync(f => f.Email.Equals(loginInfo.LoginEmail));
            return student;
        }
        public async Task<List<Class>> GetAllClass(string key)
        {
            var me = await this.GetMe(key);
            var clsses = await DataContext.StudentClassMaps.Where(f => f.StudentId == me.Id)
                .Select(f => f.Class).ToListAsync();
            return clsses;
        }
        public async Task<Class?> GetClass(int id)
        {
            return await DataContext.Classes.FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<List<Exam>> GetExams(int classId)
        {
            return await DataContext.Exams
                .Where(f=>f.ClassId== classId && f.StartTime<=DateTime.Now && f.EndTime>DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<Questions>?> GetQuestion(int examId)
        {
            var exam = await DataContext.Exams
                .FirstOrDefaultAsync(f => f.Id == examId && f.StartTime <= DateTime.Now);
            if (exam == null) return null;
            var ques = await DataContext.Questions.Where(f => f.ExamId == examId)
                .Include(f => f.Options)
                .ToListAsync();
            return ques;
        }

        public async Task<Exam?> GetExamById(int id,string appkey)
        {
            var me = await GetMe(appkey);
            var blocked = await DataContext.ExamBlocks
                .FirstOrDefaultAsync(f => f.ExamId == id && f.StudentId == me.Id);
            if (blocked != null) return null;
            return await DataContext.Exams.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> BlockMe(ExamBlock block, string appKey)
        {
            var me = await this.GetMe(appKey);
            var isBlocked = await DataContext.ExamBlocks
                .FirstOrDefaultAsync(f => f.ExamId == block.ExamId && f.StudentId == me.Id);
            if (isBlocked != null) return true;
            block.StudentId = me.Id;
            await DataContext.ExamBlocks.AddAsync(block);
            return await this.Saved();
        }
        private async Task<bool> Saved()
        {
            var save = await DataContext.SaveChangesAsync();
            return save > 0 ? true : false;
        }

        public async Task<List<ClassTask>> GetClassTask(int classId)
        {
            return await DataContext.ClassTasks.Where(f => f.ClassId == classId).ToListAsync();
        }
    }
}
