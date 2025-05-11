using CC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CC.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
    }
}