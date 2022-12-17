using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> option): base(option) { }


        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin?> Admins { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LoginInformation> LoginInformations { get; set; }
        public DbSet<StudentClassMap> StudentClassMaps { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<AnswerScript> AnswerScripts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<StudentExamMapping> StudentExamMappings { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<StudentsAnswer> StudentsAnswers { get; set; }
        public DbSet<ExamBlock> ExamBlocks { get; set; }
        public DbSet<ClassTask> ClassTasks { get; set; }
        public DbSet<StudentClassTaskDetails> StudentClassTaskDetails { get; set; }
        public DbSet<TaskMonitor> TaskMonitors { get; set; }
        public DbSet<TaskMonitorScreenShots> TaskMonitorScreenShots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginInformation>()
                .HasOne<Role>(l => l.Role)
                .WithMany(r => r.LoginInformations)
                .HasForeignKey(l => l.RoleId);
            modelBuilder.Entity<Class>()
                .HasOne<Teacher>(t => t.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(l => l.TeacherId);
            modelBuilder.Entity<StudentClassMap>()
                .HasKey(sc => new { sc.ClassId, sc.StudentId });
            modelBuilder.Entity<StudentClassMap>()
                .HasOne<Student>(s => s.Student)
                .WithMany(st => st.Classes)
                .HasForeignKey(st => st.StudentId);
            modelBuilder.Entity<StudentClassMap>()
                .HasOne<Class>(c => c.Class)
                .WithMany(st => st.Students)
                .HasForeignKey(st => st.ClassId);
        }
    }
}
