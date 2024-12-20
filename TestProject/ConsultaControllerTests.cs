using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestePratico.Controllers;
using TestePratico.Models;
using TestePratico.Data;

public class ConsultaControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult_WithListOfPessoas()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Pessoa>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockContext.Setup(c => c.Pessoas).Returns(mockSet.Object);

        var controller = new ConsultaController(mockContext.Object);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Pessoa>>(viewResult.Model);
        Assert.NotEmpty(model);
    }

    [Fact]
    public void Criar_ValidPessoa_RedirectsToIndex()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var controller = new ConsultaController(mockContext.Object);

        var pessoa = new Pessoa { NomeFantasia = "Empresa X", CnpjCpf = "12345678900" };

        // Act
        var result = controller.Criar(pessoa);

        // Assert
        mockContext.Verify(c => c.Pessoas.Add(It.IsAny<Pessoa>()), Times.Once());
        mockContext.Verify(c => c.SaveChanges(), Times.Once());
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public void Detalhe_ValidId_ReturnsViewResult()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Pessoa>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockContext.Setup(c => c.Pessoas).Returns(mockSet.Object);

        var controller = new ConsultaController(mockContext.Object);

        // Act
        var result = controller.Detalhe(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Pessoa>(viewResult.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public void Excluir_ValidId_RedirectsToIndex()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Pessoa>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockContext.Setup(c => c.Pessoas).Returns(mockSet.Object);

        var controller = new ConsultaController(mockContext.Object);

        // Act
        var result = controller.Excluir(1);

        // Assert
        mockContext.Verify(c => c.Pessoas.Remove(It.IsAny<Pessoa>()), Times.Once());
        mockContext.Verify(c => c.SaveChanges(), Times.Once());
        Assert.IsType<RedirectToActionResult>(result);
    }
}
