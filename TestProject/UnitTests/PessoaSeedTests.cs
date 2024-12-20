using System.Linq;
using TestePratico.Data;
using TestePratico.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestePratico.UnitTests
{
    public class PessoaSeedTests
    {
        [Fact] // Atributo do XUnit para indicar que o método é um teste.
        public void SeedData_ShouldPopulateDatabase_WhenEmpty()
        {
            // Arrange: Configuração inicial do teste
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Cria um banco de dados em memória único para cada teste
                .Options;

            using var context = new ApplicationDbContext(options);

            // Act: Execução da lógica a ser testada
            PessoaSeed.SeedData(context);

            // Assert: Verificação dos resultados esperados
            Assert.Equal(10, context.Pessoas.Count()); // Verifica se o número de registros é 10
            Assert.Contains(context.Pessoas, p => p.NomeFantasia == "Empresa A"); // Verifica se há pelo menos uma empresa com o nome "Empresa A".
        }

        [Fact] // Outro teste para verificar não duplicação
        public void SeedData_ShouldNotDuplicate_WhenDatabaseIsPopulated()
        {
            // Arrange: Configuração inicial do teste
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Cria um banco de dados em memória único para cada teste
                .Options;

            using var context = new ApplicationDbContext(options);

            PessoaSeed.SeedData(context); // Chamada para adicionar os dados iniciais

            // Act: Executa o método de seeding novamente
            PessoaSeed.SeedData(context); // Tentativa de seeding novamente

            // Assert: Verifica se não há duplicações de registros
            Assert.Equal(10, context.Pessoas.Count()); // Confirma que o número de registros continua sendo 10
        }
    }
}