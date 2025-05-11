


using CC.Domain.Entities;
using CC.Domain.Interfaces;
using CC.Infrastructure.DatabaseContext;
using CC.Infrastructure.Repositories.Generics;

namespace CC.Infrastructure.Repositories.AccountRepository
{
    class UserRepository : GRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
