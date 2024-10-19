using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Entities;

namespace TechChallengeFaseUm.Repositories
{
    public class DbContextRepository : DbContext
    {
        public DbContextRepository(DbContextOptions<DbContextRepository> options) : base(options)
        {
        }

        public DbSet<ContatosResponse> contatos { get; set; }
    }
}
