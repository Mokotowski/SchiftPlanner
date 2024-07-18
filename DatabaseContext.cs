using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Models.Survey;
using System.Reflection.Emit;

namespace SchiftPlanner
{
    public class DatabaseContext : IdentityDbContext<UserModel>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<Type_Subscriptions> Type_Subscriptions { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<Opinions> Opinions { get; set; }
        public DbSet<SurveysProperties> SurveysProperties { get; set; }



        public DbSet<Company_Type1> Company_Type1 { get; set; }
        public DbSet<Worker_Timetable> Worker_Timetable { get; set; }
        public DbSet<Day_Worker_Timetable> Day_Worker_Timetable { get; set; }
        public DbSet<Day_Worker_Claimed> Day_Worker_Claimed { get; set; }
        public DbSet<Workers> Workers { get; set; }




        public DbSet<Company_Type2> Company_Type2 { get; set; }
        public DbSet<Customer_Timetable> Customer_Timetable { get; set; }
        public DbSet<Day_Customer_Timetable> Day_Customer_Timetable { get; set; }
        public DbSet<Day_Customer_Claimed> Day_Customer_Claimed { get; set; }


        


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Subs
            builder.Entity<Subscriptions>()
            .HasOne(s => s.User)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.Id_User);

            builder.Entity<Subscriptions>()
            .HasOne(s => s.Type_Subscriptions)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.Id_Sub);



            builder.Entity<Subscriptions>()
            .HasOne(s => s.CompanyInfo)
            .WithOne(c => c.Subscriptions)
            .HasForeignKey<CompanyInfo>(s => s.Id_Company);



            // Subs and Company
            builder.Entity<CompanyInfo>()
            .HasOne(s => s.Company_Type1)
            .WithOne(c => c.Company_Info)
            .HasForeignKey<Company_Type1>(s => s.Id_Company);

            builder.Entity<CompanyInfo>()
            .HasOne(s => s.Company_Type2)
            .WithOne(c => c.Company_Info)
            .HasForeignKey<Company_Type2>(s => s.Id_Company);



            // Opinions
            builder.Entity<Opinions>()
            .HasOne(s => s.CompanyInfo)
            .WithMany(c => c.Opinions)
            .HasForeignKey(s => s.Id_Company);



            // Company_Type1

            builder.Entity<Workers>()
            .HasOne(w => w.Company_Type1)
            .WithMany(c => c.Workers)
            .HasForeignKey(w => w.Company_Type1Id)
            .IsRequired();

            builder.Entity<Worker_Timetable>()
            .HasOne(s => s.Company_Type1)
            .WithMany(c => c.Worker_Timetables)
            .HasForeignKey(s => s.Id_Company);

            builder.Entity<Day_Worker_Timetable>()
            .HasOne(s => s.Worker_Timetable)
            .WithMany(c => c.Worker_Days)
            .HasForeignKey(s => s.Id_Timetable);



            // Company_Type2

            builder.Entity<Customer_Timetable>()
            .HasOne(s => s.Company_Type2)
            .WithMany(c => c.Customer_Timetables)
            .HasForeignKey(s => s.Id_Company);

            builder.Entity<Day_Customer_Timetable>()
            .HasOne(s => s.Customer_Timetable)
            .WithMany(c => c.Customer_Days)
            .HasForeignKey(s => s.Id_Timetable);



            // Survey
            builder.Entity<SurveysProperties>()
            .HasOne(s => s.CompanyInfo)
            .WithMany(c => c.SurveysProperties)
            .HasForeignKey(s => s.Id_Company);



            builder.Entity<Question>()
            .HasOne(s => s.SurveysProperties)
            .WithMany(c => c.Questions)
            .HasForeignKey(s => s.Id_Survey);


            builder.Entity<Question>()
            .HasOne(s => s.SurveysProperties)
            .WithMany(c => c.Questions)
            .HasForeignKey(s => s.Id_Survey);

            builder.Entity<Option_Answer>()
            .HasOne(s => s.Question)
            .WithMany(c => c.Options_Answer)
            .HasForeignKey(s => s.Question_Location);





            // Day Plans
            builder.Entity<Day_Worker_Claimed>()
            .HasOne(s => s.Day_Worker_Timetable)
            .WithMany(c => c.Day_Plan_Claimed)
            .HasForeignKey(s => s.Timetable_Day);


            builder.Entity<Day_Customer_Claimed>()
            .HasOne(s => s.Day_Customer_Timetable)
            .WithMany(c => c.Day_Plan_Claimed)
            .HasForeignKey(s => s.Timetable_Day);







            base.OnModelCreating(builder);
        }
    }
}




