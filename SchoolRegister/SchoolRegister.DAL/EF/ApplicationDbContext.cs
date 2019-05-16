using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.BLL.Entities;

namespace SchoolRegister.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly ConnectionStringDto _connectionStringDto;

        // Table properties e.g
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectGroup> SubjectGroup { get; set; }


        public ApplicationDbContext(ConnectionStringDto connectionStringDto)
        {
            _connectionStringDto = connectionStringDto;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_connectionStringDto.ConnectionString); // for provider SQL Server 
            // optionsBuilder.UseMySql(_connectionStringDto.ConnectionString); //for provider My SQL 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API commands
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(x => x.GroupId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers")
                .HasDiscriminator<int>("UserType")
                .HasValue<Student>(1)
                .HasValue<Parent>(2)
                .HasValue<Teacher>(3);


        }
    }
}