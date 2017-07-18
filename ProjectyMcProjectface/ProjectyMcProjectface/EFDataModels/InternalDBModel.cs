namespace ProjectyMcProjectface
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InternalDBModel : DbContext
    {
        public InternalDBModel()
            : base(@"Data Source=.\SQLEXPRESS;Database=InternalDB;Integrated Security=True")
        {
        }

        public virtual DbSet<RegisteredUser> RegisteredUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<RegisteredUser>()
                .Property(e => e.PassWord)
                .IsUnicode(false);
        }

        public void RegisterUser(string UserName, string Password)
        {
            InternalDBModel context = new InternalDBModel();
            context.RegisteredUsers.Add(new RegisteredUser() { UserName = UserName, PassWord = Password });
            context.SaveChanges();
        }
    }
}
