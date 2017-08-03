namespace EFDataModels
{
    using System.Data.Entity;

    public interface IInternalDBModel
    {
        int SaveChanges();
        DbSet<RegisteredUser> RegisteredUsers { get; set; }
        DbSet<ConnectionString> ConnectionStrings { get; set; }
    }
    public partial class InternalDBContext : DbContext, IInternalDBModel
    {
        public InternalDBContext()
            : base(@"Data Source=.\SQLEXPRESS;Database=InternalDB;Integrated Security=True")
        {
        }

        public virtual DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public virtual DbSet<ConnectionString> ConnectionStrings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<RegisteredUser>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<RegisteredUser>()
                .HasMany(x => x.ConnectionStrings)
                .WithRequired(x => x.User)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<ConnectionString>().HasKey(x => x.ConnectionId);

        }
        
    }
}
