﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entity;
using Entity.DTO;
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

        public async Task<List<Class>> GetAllClass(string key)
        {
            var me = await this.GetMe(key);
            var classes = await this.Data.Classes
                .Where(f => f.TeacherId == me.Id).ToListAsync();
            return classes;
        }

        public async Task<Class?> GetClass(int id)
        {
            return await Data.Classes.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Student>> GetStudents(int id)
        {
            var students = from s in Data.Students
                where !(from c in s.Classes
                        select c.ClassId)
                    .Contains(id)
                select s;
            return await students.ToListAsync();
        }

        public async Task<List<StudentClassDTO>> SaveStudentsToClass(List<StudentClassDTO> student, int classId)
        {
            List<StudentClassMap> maps = new List<StudentClassMap>();

            Parallel.ForEach(student, s =>
            {
                maps.Add(this.SaveStudentsAsync(s, classId));
            });
            await Data.StudentClassMaps.AddRangeAsync(maps);
            await this.Saved();
            return student;
        }

        private  StudentClassMap SaveStudentsAsync(StudentClassDTO student, int classId)
        {
            var map = new StudentClassMap()
            {
                StudentId = student.Id,
                ClassId = classId
            };
           return map;
        }

        public async Task<List<Student>> GetClassStudents(int classId)
        {
            var students =  from s in Data.Students
                where (from c in s.Classes select c.ClassId).Contains(classId)
                    select s;
            return await students.ToListAsync();
        }

        public async Task<bool> RemoveStudentFromClass(int sId, int classId)
        {
            var classMap = await Data.StudentClassMaps
                .FirstOrDefaultAsync(f=>f.ClassId==classId && f.StudentId==sId);
            Data.StudentClassMaps.Remove(classMap);
            return await this.Saved();
        }
    }
}
