namespace CC.Infrastructure.DatabaseContext
{
    using CC.Application.Interfaces;
    using CC.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
