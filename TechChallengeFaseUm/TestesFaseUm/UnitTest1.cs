using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Controllers;
using TechChallengeFaseUm.Repositories;
using TechChallengeFaseUm.Entities;
using TestesFaseUm.Tests.Repositories;

public class ContatosControllerTests
{
    private readonly ContatosController _controller;
    private readonly TestDbContextRepository _context;

    public ContatosControllerTests()
    {
        var options = new DbContextOptionsBuilder<DbContextRepository>()
                      .UseInMemoryDatabase(databaseName: "TestDatabase")
                      .Options;
        _context = new TestDbContextRepository(options);
        _controller = new ContatosController(_context);
    }

    [Fact]
    public async Task CreateContact_ReturnsOkResult_WithInsertedContact()
    {
        var contatosRequest = new ContatosRequest
        {
            name = "John Doe",
            email = "john.doe@example.com",
            telefone = "(11) 91234-5678"
        };

        var result = await _controller.CreateContact(contatosRequest) as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Dados inseridos com sucesso!", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var novoContato = returnedValue.Data;
        Assert.NotNull(novoContato);
        Assert.Equal(contatosRequest.name, novoContato.name);
        Assert.Equal(contatosRequest.email, novoContato.email);
        Assert.Equal(contatosRequest.telefone, novoContato.telefone);
    }

    [Fact]
    public async Task GetAll_ReturnsAllContacts()
    {
        var result = await _controller.GetAll() as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<IEnumerable<ContatosResponse>>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contatos retrieved successfully", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var contacts = returnedValue.Data;
        Assert.NotNull(contacts);
    }

    [Fact]
    public async Task GetByDDD_ReturnsFilteredContacts()
    {
        // Limpa o contexto antes de adicionar novos itens
        _context.contatos.RemoveRange(_context.contatos);
        await _context.SaveChangesAsync();

        // Adiciona contatos de teste
        var contatos = new List<ContatosResponse>
        {
            new ContatosResponse { id = "1", name = "John Doe", email = "john.doe@example.com", telefone = "(11) 91234-5678" },
            new ContatosResponse { id = "2", name = "Jane Doe", email = "jane.doe@example.com", telefone = "(21) 98765-4321" }
        };

        _context.contatos.AddRange(contatos);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetByDDD(11) as OkObjectResult;
        Assert.NotNull(result);

        // Assert
        var returnedValue = result.Value as ApiResponse<IEnumerable<ContatosResponse>>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Filtered contatos retrieved successfully", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var filteredContacts = returnedValue.Data;
        Assert.NotNull(filteredContacts);
        Assert.Single(filteredContacts);
        Assert.Equal("(11) 91234-5678", filteredContacts.First().telefone);
    }

    [Fact]
    public async Task DeleteResourceById_ReturnsOkResult_WithDeletedContact()
    {
        // Limpa o contexto antes de adicionar novos itens
        _context.contatos.RemoveRange(_context.contatos);
        await _context.SaveChangesAsync();

        var contato = new ContatosResponse { id = "1", name = "John Doe", email = "john.doe@example.com", telefone = "(11) 91234-5678" };
        _context.contatos.Add(contato);
        await _context.SaveChangesAsync();

        var result = _controller.DeleteResourceById("1") as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contato com ID 1 excluído com sucesso!", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var deletedContato = returnedValue.Data;
        Assert.NotNull(deletedContato);
        Assert.Equal(contato.name, deletedContato.name);
        Assert.Equal(contato.email, deletedContato.email);
        Assert.Equal(contato.telefone, deletedContato.telefone);
    }


    [Fact]
    public void UpdateResource_ReturnsOkResult_WithUpdatedContact()
    {
        var contato = new ContatosResponse { id = "1", name = "John Doe", email = "john.doe@example.com", telefone = "(11) 91234-5678" };
        _context.contatos.Add(contato);
        _context.SaveChanges();

        var updatedContato = new ContatosRequest { name = "Johnny Doe", email = "johnny.doe@example.com" };

        var result = _controller.UpdateResource("1", updatedContato) as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contato com ID 1 atualizado com sucesso!", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var contatoAtualizado = returnedValue.Data;
        Assert.NotNull(contatoAtualizado);
        Assert.Equal("Johnny Doe", contatoAtualizado.name);
        Assert.Equal("johnny.doe@example.com", contatoAtualizado.email);
        Assert.Equal("(11) 91234-5678", contatoAtualizado.telefone); // Telefone não alterado
    }
}
