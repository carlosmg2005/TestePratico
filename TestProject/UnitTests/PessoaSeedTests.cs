using System.Linq;
using TestePratico.Data;
using TestePratico.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestePratico.UnitTests
{
    public class PessoaSeedTests
    {
        [Fact]
        public void SeedData_ShouldPopulateDatabase_WhenEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Banco isolado
                .Options;

            using var context = new ApplicationDbContext(options);

            // Act
            PessoaSeed.SeedData(context);

            // Assert
            Assert.Equal(10, context.Pessoas.Count());
            Assert.Contains(context.Pessoas, p => p.NomeFantasia == "Empresa A");
        }

        [Fact]
        public void SeedData_ShouldNotDuplicate_WhenDatabaseIsPopulated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            PessoaSeed.SeedData(context);

            // Act
            PessoaSeed.SeedData(context);

            // Assert
            Assert.Equal(10, context.Pessoas.Count());
        }
    }
}
