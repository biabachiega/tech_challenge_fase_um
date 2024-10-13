using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TechChallengeFaseUm.Entities;
using TechChallengeFaseUm.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static TechChallengeFaseUm.Repositories.DbContextRepository;

namespace TechChallengeFaseUm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeuTesteController : ControllerBase
    {
        private readonly DbContextRepository _dbContext;

        public MeuTesteController(DbContextRepository dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] NewTableTeste product)
        {
            try
            {
                // Crie uma nova entidade com os dados do modelo
                var entity = new NewTableTeste
                {
                    name = product.name,
                    id = product.id
                };

                // Adicione a entidade ao contexto e salve as mudanças
                _dbContext.Set<NewTableTeste>().Add(entity);
                await _dbContext.SaveChangesAsync();

                return Ok("Dados inseridos com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao inserir dados: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewTableTeste>>> GetNewTableTeste()
        {
            return await _dbContext.newtableteste.ToListAsync();
        }

    }
}
