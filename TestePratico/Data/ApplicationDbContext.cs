using Microsoft.EntityFrameworkCore;
using TestePratico.Models;

namespace TestePratico.Data
{
    // Contexto de banco de dados utilizado pela aplicação
    public class ApplicationDbContext : DbContext
    {
        // Conjunto de dados de pessoas que será mapeado para a tabela 'Pessoas'
        public DbSet<Pessoa> Pessoas { get; set; }

        // Construtor que recebe os options necessários para configurar o contexto
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
