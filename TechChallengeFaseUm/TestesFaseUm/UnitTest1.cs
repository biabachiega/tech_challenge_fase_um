using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechChallengeFaseUm.Controllers;
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
            nome = "Wally West",
            email = "wally.west@youngJL.com",
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
        Assert.Equal(contatosRequest.nome, novoContato.nome);
        Assert.Equal(contatosRequest.email, novoContato.email);
        Assert.Equal(contatosRequest.telefone, novoContato.telefone);
    }
    [Fact]
    public async Task CreateContact_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var contatosRequest = new ContatosRequest
        {
            nome = "",
            email = "wally.west@youngJL.com",
            telefone = "(11) 91234-5678"
        };

        _controller.ModelState.AddModelError("nome", "Nome � obrigat�rio.");

        var result = await _controller.CreateContact(contatosRequest) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("O estado do modelo n�o � v�lido", returnedValue.Message);
        Assert.True(returnedValue.HasError);
        Assert.Null(returnedValue.Data);
    }


    [Fact]
    public async Task GetAll_ReturnsAllContacts()
    {
        var result = await _controller.GetAll() as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<IEnumerable<ContatosResponse>>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contatos obtidos com sucesso", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var contacts = returnedValue.Data;
        Assert.NotNull(contacts);
    }

    [Fact]
    public async Task GetByDDD_ReturnsFilteredContacts()
    {
        _context.contatos.RemoveRange(_context.contatos);
        await _context.SaveChangesAsync();

        var contatos = new List<ContatosResponse>
        {
            new ContatosResponse { id = "e8837348-64d2-4602-bf86-8744dce4ec65", nome = "Wally West", email = "wally.west@youngJL.com", telefone = "(11) 91234-5678" },
            new ContatosResponse { id = "b8f105bd-3546-4b05-bc13-ecedce4a3f8e", nome = "Artemis Crock", email = "Artemis.crock@example.com", telefone = "(21) 98765-4321" }
        };

        _context.contatos.AddRange(contatos);
        await _context.SaveChangesAsync();

        var result = await _controller.GetByDDD(11) as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<IEnumerable<ContatosResponse>>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contatos filtrados obtidos com sucesso", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var filteredContacts = returnedValue.Data;
        Assert.NotNull(filteredContacts);
        Assert.Single(filteredContacts);
        Assert.Equal("(11) 91234-5678", filteredContacts.First().telefone);
    }

    [Fact]
    public async Task DeleteResourceById_ReturnsOkResult_WithDeletedContact()
    {
        _context.contatos.RemoveRange(_context.contatos);
        await _context.SaveChangesAsync();

        var contato = new ContatosResponse { id = "e8837348-64d2-4602-bf86-8744dce4ec65", nome = "Wally West", email = "wally.west@youngJL.com", telefone = "(11) 91234-5678" };
        _context.contatos.Add(contato);
        await _context.SaveChangesAsync();

        var result = _controller.DeleteResourceById("e8837348-64d2-4602-bf86-8744dce4ec65") as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contato com Id e8837348-64d2-4602-bf86-8744dce4ec65 exclu�do com sucesso!", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var deletedContato = returnedValue.Data;
        Assert.NotNull(deletedContato);
        Assert.Equal(contato.nome, deletedContato.nome);
        Assert.Equal(contato.email, deletedContato.email);
        Assert.Equal(contato.telefone, deletedContato.telefone);
    }


    [Fact]
    public void UpdateResource_ReturnsOkResult_WithUpdatedContact()
    {
        var contato = new ContatosResponse { id = "e8837348-64d2-4602-bf86-8744dce4ec65", nome = "Wally West", email = "wally.west@youngJL.com", telefone = "(11) 91234-5678" };
        _context.contatos.Add(contato);
        _context.SaveChanges();

        var updatedContato = new ContatosUpdateRequest { nome = "Wally", email = "kid.flash@youngJL.com" };

        var result = _controller.UpdateResource("e8837348-64d2-4602-bf86-8744dce4ec65", updatedContato) as OkObjectResult;
        Assert.NotNull(result);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.Equal("Contato com Id e8837348-64d2-4602-bf86-8744dce4ec65 atualizado com sucesso!", returnedValue.Message);
        Assert.False(returnedValue.HasError);

        var contatoAtualizado = returnedValue.Data;
        Assert.NotNull(contatoAtualizado);
        Assert.Equal("Wally", contatoAtualizado.nome);
        Assert.Equal("kid.flash@youngJL.com", contatoAtualizado.email);
        Assert.Equal("(11) 91234-5678", contatoAtualizado.telefone);
    }
    [Fact]
    public void UpdateResource_ReturnsBadRequest_WhenInvalidRequest()
    {
        var invalidContato = new ContatosUpdateRequest { nome = "", email = "invalid-email", telefone = "" };
        var controller = new ContatosController(_context);
        controller.ModelState.AddModelError("nome", "Nome � obrigat�rio.");
        controller.ModelState.AddModelError("email", "Email em formato inv�lido.");
        controller.ModelState.AddModelError("telefone", "Telefone � obrigat�rio.");

        var result = controller.UpdateResource("e8837348-64d2-4602-bf86-8744dce4ec65", invalidContato) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);

        var returnedValue = result.Value as ApiResponse<ContatosResponse>;
        Assert.NotNull(returnedValue);
        Assert.True(returnedValue.HasError);
        Assert.Equal("Dados inv�lidos fornecidos.", returnedValue.Message);
    }


}
