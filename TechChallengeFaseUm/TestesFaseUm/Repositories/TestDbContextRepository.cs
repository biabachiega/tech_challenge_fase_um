using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallengeFaseUm.Entities;

namespace TestesFaseUm.Tests.Repositories
{
    public class TestDbContextRepository : DbContextRepository
    {
        public TestDbContextRepository(DbContextOptions<DbContextRepository> options)
            : base(options)
        {
        }
    }
}
