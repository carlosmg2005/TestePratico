using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestePratico.Controllers;
using TestePratico.Data;
using TestePratico.Models;
using Xunit;

namespace TestePratico.IntegrationTests
{
    public class ConsultaControllerTests
    {
        // Método para criar um contexto em memória para testes
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Cria um novo banco em memória para cada teste
                .Options;

            return new ApplicationDbContext(options);
        }

        // Testa se o método Index retorna uma View com a lista de Pessoas
        [Fact]
        public async Task Index_ShouldReturnViewWithPessoas()
        {
            // Arrange: Configuração inicial do contexto e inclusão de uma pessoa no banco de dados
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted();
            context.Pessoas.Add(new Pessoa { NomeFantasia = "Teste", CnpjCpf = "12345678900" });
            await context.SaveChangesAsync();

            var controller = new ConsultaController(context);

            // Act: Chamada ao método Index
            var result = controller.Index() as ViewResult;

            // Assert: Verificação dos resultados esperados
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as List<Pessoa>;
            Assert.Single(model);
            Assert.Equal("Teste", model.First().NomeFantasia);
        }

        // Testa se o método Criar adiciona uma nova Pessoa quando o modelo é válido
        [Fact]
        public void Criar_Post_ShouldAddPessoa_WhenModelIsValid()
        {
            // Arrange: Configuração inicial do contexto e criação de uma nova pessoa
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted(); // Limpa o banco antes de cada teste
            var controller = new ConsultaController(context);
            var pessoa = new Pessoa { NomeFantasia = "Nova Empresa", CnpjCpf = "98765432100" };

            // Act: Chamada ao método Criar
            var result = controller.Criar(pessoa);

            // Assert: Verificação dos resultados esperados
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(context.Pessoas);
            Assert.Equal("Nova Empresa", context.Pessoas.First().NomeFantasia);
        }

        // Testa se o método Detalhe retorna uma View com detalhes de uma Pessoa existente
        [Fact]
        public void Detalhe_Get_ShouldReturnViewWithPessoa_WhenPessoaExists()
        {
            // Arrange: Configuração inicial com adição de uma pessoa ao contexto
            var context = GetInMemoryDbContext();
            var pessoa = new Pessoa { NomeFantasia = "Detalhe Empresa", CnpjCpf = "12345678901" };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            var controller = new ConsultaController(context);

            // Act: Chamada ao método Detalhe
            var result = controller.Detalhe(pessoa.PessoaId) as ViewResult;

            // Assert: Verificação dos resultados esperados
            Assert.NotNull(result);
            Assert.IsType<Pessoa>(result.Model);
            Assert.Equal("Detalhe Empresa", ((Pessoa)result.Model).NomeFantasia);
        }

        // Testa se o método Detalhe retorna NotFound quando a Pessoa não existe
        [Fact]
        public void Detalhe_Get_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            // Arrange: Configuração inicial do contexto
            var context = GetInMemoryDbContext();
            var controller = new ConsultaController(context);

            // Act: Chamada ao método Detalhe com um ID inexistente
            var result = controller.Detalhe(999); // ID inexistente

            // Assert: Verificação dos resultados esperados
            Assert.IsType<NotFoundResult>(result);
        }

        // Testa se o método Detalhe Post atualiza uma Pessoa quando o modelo é válido
        [Fact]
        public void Detalhe_Post_ShouldUpdatePessoa_WhenModelIsValid()
        {
            // Arrange: Configuração inicial com adição de uma pessoa ao contexto
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted();
            var pessoa = new Pessoa { NomeFantasia = "Detalhe Empresa", CnpjCpf = "12345678901" };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            var controller = new ConsultaController(context);

            // Act: Atualização da Pessoa através do método Detalhe Post
            pessoa.NomeFantasia = "Empresa Atualizada";
            var result = controller.Detalhe(pessoa) as ViewResult;

            // Assert: Verificação dos resultados esperados
            Assert.NotNull(result);
            var updatedPessoa = context.Pessoas.First();
            Assert.Equal("Empresa Atualizada", updatedPessoa.NomeFantasia);
            Assert.Equal("Alterações salvas com sucesso!", controller.ViewBag.Message);
        }

        // Testa se o método Detalhe Post retorna uma View com o modelo inválido
        [Fact]
        public void Detalhe_Post_ShouldReturnViewWithModel_WhenModelIsInvalid()
        {
            // Arrange: Configuração inicial com adição de uma pessoa ao contexto e erro de validação
            var context = GetInMemoryDbContext();
            var pessoa = new Pessoa { NomeFantasia = "Empresa Inválida", CnpjCpf = "12345678901" };
            var controller = new ConsultaController(context);

            controller.ModelState.AddModelError("NomeFantasia", "Erro no Nome");

            // Act: Atualização da Pessoa através do método Detalhe Post
            var result = controller.Detalhe(pessoa) as ViewResult;

            // Assert: Verificação dos resultados esperados
            Assert.NotNull(result);
            Assert.Equal(pessoa, result.Model);
        }

        // Testa se o método Excluir remove uma Pessoa quando ela existe
        [Fact]
        public void Excluir_Post_ShouldRemovePessoa_WhenPessoaExists()
        {
            // Arrange: Configuração inicial com adição de uma pessoa ao contexto
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var pessoa = new Pessoa { NomeFantasia = "Empresa a Excluir", CnpjCpf = "98765432101" };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            var controller = new ConsultaController(context);

            // Configuração manual do TempData
            var tempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
            controller.TempData = tempData;

            // Act: Exclusão da Pessoa através do método Excluir
            var result = controller.Excluir(pessoa.PessoaId);

            // Assert: Verificação dos resultados esperados
            Assert.NotNull(result); // Verifica que há um resultado
            Assert.IsType<RedirectToActionResult>(result); // Deve redirecionar para Index
            Assert.Empty(context.Pessoas); // Banco deve estar vazio após exclusão
            Assert.Equal("Pessoa excluída com sucesso!", controller.TempData["SuccessMessage"]);
        }

        // Testa se o método Excluir retorna NotFound quando a Pessoa não existe
        [Fact]
        public void Excluir_Post_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            // Arrange: Configuração inicial do contexto
            var context = GetInMemoryDbContext();
            var controller = new ConsultaController(context);

            // Act: Chamada ao método Excluir com um ID inexistente
            var result = controller.Excluir(999); // ID inexistente

            // Assert: Verificação dos resultados esperados
            Assert.IsType<NotFoundResult>(result);
        }
    }
}