using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Entities;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ContatosResponse>
                {
                    Message = "O estado do modelo não é válido",
                    HasError = true,
                    Data = null
                });
            }

            try
            {
                var novoContato = new ContatosResponse
                {
                    id = Guid.NewGuid().ToString(),
                    name = contatos.name,
                    email = contatos.email,
                    telefone = contatos.telefone
                };

                _dbContext.Set<ContatosResponse>().Add(novoContato);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse<ContatosResponse>
                {
                    Message = "Dados inseridos com sucesso!",
                    HasError = false,
                    Data = novoContato
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ContatosResponse>
                {
                    Message = $"Erro ao inserir dados: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await _dbContext.contatos.ToListAsync();
                return Ok(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = "Contatos recuperado com sucesso",
                    HasError = false,
                    Data = contacts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = $"Erro ao recuperar contatos: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

        [HttpGet("getByDDD/{ddd}")]
        public async Task<IActionResult> GetByDDD(int ddd)
        {
            try
            {
                var filteredContacts = await _dbContext.contatos
                    .Where(c => c.telefone.StartsWith($"({ddd})"))
                    .ToListAsync();

                return Ok(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = "Contatos filtrados recuperados com sucesso",
                    HasError = false,
                    Data = filteredContacts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = $"Erro ao recuperar contatos filtrados: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

        [HttpDelete("deleteById/{id}")]
        public IActionResult DeleteResourceById(string id)
        {
            try
            {
                var entityToDelete = _dbContext.contatos.FirstOrDefault(c => c.id == id);
                if (entityToDelete != null)
                {
                    _dbContext.contatos.Remove(entityToDelete);
                    _dbContext.SaveChanges();

                    return Ok(new ApiResponse<ContatosResponse>
                    {
                        Message = $"Contato com ID {id} excluído com sucesso!",
                        HasError = false,
                        Data = entityToDelete
                    });
                }
                else
                {
                    return NotFound(new ApiResponse<ContatosResponse>
                    {
                        Message = $"Contato com ID {id} não encontrado.",
                        HasError = true,
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ContatosResponse>
                {
                    Message = $"Erro ao excluir contato: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

        [HttpPut("updateById/{id}")]
        public IActionResult UpdateResource(string id, [FromBody] ContatosUpdateRequest updatedResource)
        {
            try
            {
                var existingResource = _dbContext.contatos.FirstOrDefault(c => c.id == id);
                if (existingResource == null)
                {
                    return NotFound(new ApiResponse<ContatosResponse>
                    {
                        Message = $"Recurso com ID {id} não encontrado.",
                        HasError = true
                    });
                }

                existingResource.name = updatedResource.name ?? existingResource.name;
                existingResource.telefone = updatedResource.telefone ?? existingResource.telefone;
                existingResource.email = updatedResource.email ?? existingResource.email;

                _dbContext.SaveChanges();

                return Ok(new ApiResponse<ContatosResponse>
                {
                    Message = $"Contato com ID {id} atualizado com sucesso!",
                    HasError = false,
                    Data = existingResource
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ContatosResponse>
                {
                    Message = $"Erro ao atualizar contato: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

    }
}
