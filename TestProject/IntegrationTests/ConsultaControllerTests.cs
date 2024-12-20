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
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Cria um novo banco em memória para cada teste
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPessoas()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted();
            context.Pessoas.Add(new Pessoa { NomeFantasia = "Teste", CnpjCpf = "12345678900" });
            await context.SaveChangesAsync();

            var controller = new ConsultaController(context);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as List<Pessoa>;
            Assert.Single(model);
            Assert.Equal("Teste", model.First().NomeFantasia);
        }

        [Fact]
        public void Criar_Post_ShouldAddPessoa_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted(); // Limpa o banco antes de cada teste
            var controller = new ConsultaController(context);
            var pessoa = new Pessoa { NomeFantasia = "Nova Empresa", CnpjCpf = "98765432100" };

            // Act
            var result = controller.Criar(pessoa);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(context.Pessoas);
            Assert.Equal("Nova Empresa", context.Pessoas.First().NomeFantasia);
        }

        [Fact]
        public void Detalhe_Get_ShouldReturnViewWithPessoa_WhenPessoaExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var pessoa = new Pessoa { NomeFantasia = "Detalhe Empresa", CnpjCpf = "12345678901" };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            var controller = new ConsultaController(context);

            // Act
            var result = controller.Detalhe(pessoa.PessoaId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Pessoa>(result.Model);
            Assert.Equal("Detalhe Empresa", ((Pessoa)result.Model).NomeFantasia);
        }

        [Fact]
        public void Detalhe_Get_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ConsultaController(context);

            // Act
            var result = controller.Detalhe(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Detalhe_Post_ShouldUpdatePessoa_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Database.EnsureDeleted();
            var pessoa = new Pessoa { NomeFantasia = "Detalhe Empresa", CnpjCpf = "12345678901" };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            var controller = new ConsultaController(context);

            // Act
            pessoa.NomeFantasia = "Empresa Atualizada";
            var result = controller.Detalhe(pessoa) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var updatedPessoa = context.Pessoas.First();
            Assert.Equal("Empresa Atualizada", updatedPessoa.NomeFantasia);
            Assert.Equal("Alterações salvas com sucesso!", controller.ViewBag.Message);
        }

        [Fact]
        public void Detalhe_Post_ShouldReturnViewWithModel_WhenModelIsInvalid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var pessoa = new Pessoa { NomeFantasia = "Empresa Inválida", CnpjCpf = "12345678901" };
            var controller = new ConsultaController(context);

            controller.ModelState.AddModelError("NomeFantasia", "Erro no Nome");

            // Act
            var result = controller.Detalhe(pessoa) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pessoa, result.Model);
        }

        [Fact]
        public void Excluir_Post_ShouldRemovePessoa_WhenPessoaExists()
        {
            // Arrange
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

            // Act
            var result = controller.Excluir(pessoa.PessoaId);

            // Assert
            Assert.NotNull(result); // Verifica que há um resultado
            Assert.IsType<RedirectToActionResult>(result); // Deve redirecionar para Index
            Assert.Empty(context.Pessoas); // Banco deve estar vazio após exclusão
            Assert.Equal("Pessoa excluída com sucesso!", controller.TempData["SuccessMessage"]);
        }


        [Fact]
        public void Excluir_Post_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ConsultaController(context);

            // Act
            var result = controller.Excluir(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}