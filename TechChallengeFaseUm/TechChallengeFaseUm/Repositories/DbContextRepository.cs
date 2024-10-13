using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Entities;

namespace TechChallengeFaseUm.Repositories
{
    public class DbContextRepository : DbContext
    {
        public DbContextRepository(DbContextOptions<DbContextRepository> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=172.17.0.3;Port=5432;Database=postgres;User Id=postgres;Password=1234");
        }

        // DbSet para a tabela newtableteste
        public DbSet<NewTableTeste> newtableteste { get; set; }

        public DbSet<ContatosRequest> contatos { get; set; }
    }
}
