using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Entities;

public class DbContextRepository : DbContext
{
    public DbContextRepository(DbContextOptions<DbContextRepository> options) : base(options)
    {
    }

    public DbSet<ContatosResponse> contatos { get; set; }
}
