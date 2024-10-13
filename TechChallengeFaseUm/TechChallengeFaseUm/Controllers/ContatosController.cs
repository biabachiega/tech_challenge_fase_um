using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Entities;
using TechChallengeFaseUm.Repositories;

namespace TechChallengeFaseUm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatosController : Controller
    {
        private readonly DbContextRepository _dbContext;

        public ContatosController(DbContextRepository dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContatosRequest contatos)
        {
            try
            {
                var guidId = Guid.NewGuid();
                contatos.id = guidId.ToString();
                // Adicione a entidade ao contexto e salve as mudanças
                _dbContext.Set<ContatosRequest>().Add(contatos);
                await _dbContext.SaveChangesAsync();

                return Ok("Dados inseridos com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao inserir dados: {ex.Message}");
            }
        }



        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<ContatosRequest>>> GetAll()
        {
            return await _dbContext.contatos.ToListAsync();

        }


        [HttpGet("getByDDD/{ddd}")]
        public async Task<ActionResult<IEnumerable<ContatosRequest>>> GetByDDD(int ddd)
        {
            var filteredContacts = await _dbContext.contatos
                .Where(c => c.telefone.StartsWith(ddd.ToString())) // Filtra os telefones que começam com "11"
                .ToListAsync();

            return filteredContacts;

        }

        [HttpDelete("deleteById/{id}")]
        public async Task<ActionResult<IEnumerable<ContatosRequest>>> DeleteResourceById(string id)
        {
            var entityToDelete = _dbContext.contatos.FirstOrDefault(c => c.id == id);
            if (entityToDelete != null)
            {
                _dbContext.contatos.Remove(entityToDelete);
                _dbContext.SaveChanges();
                return Ok($"Contato com ID {id} excluído com sucesso!");
            }
            else
            {
                return NotFound($"Contato com ID {id} não encontrado.");
            }
        }


        [HttpPut("updateById/{id}")]
        public IActionResult UpdateResource(string id, [FromBody] ContatosRequest updatedResource)
        {
            var existingResource = _dbContext.contatos.FirstOrDefault(c => c.id == id);
            if (existingResource == null)
                return NotFound($"Recurso com ID {id} não encontrado.");

            // Atualize apenas os campos desejados
            existingResource.name = updatedResource.name;
            existingResource.telefone = updatedResource.telefone;
            existingResource.email = updatedResource.email;

            _dbContext.SaveChanges();

            return Ok($"Contato com ID {id} atualizado com sucesso!");
        }


    }
}
