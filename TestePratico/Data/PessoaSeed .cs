using System.Linq;
using TestePratico.Models;
using TestePratico.Data;

namespace TestePratico
{
    public static class PessoaSeed
    {
        public static void SeedData(ApplicationDbContext context)
        {
            if (!context.Pessoas.Any())
            {
                context.Pessoas.AddRange(
                    new Pessoa { NomeFantasia = "Empresa A", CnpjCpf = "12345678900" },
                    new Pessoa { NomeFantasia = "Empresa B", CnpjCpf = "23456789012" },
                    new Pessoa { NomeFantasia = "Empresa C", CnpjCpf = "34567890123" },
                    new Pessoa { NomeFantasia = "Empresa D", CnpjCpf = "45678901234" },
                    new Pessoa { NomeFantasia = "Empresa E", CnpjCpf = "56789012345" },
                    new Pessoa { NomeFantasia = "Empresa F", CnpjCpf = "67890123456" },
                    new Pessoa { NomeFantasia = "Empresa G", CnpjCpf = "78901234567" },
                    new Pessoa { NomeFantasia = "Empresa H", CnpjCpf = "89012345678" },
                    new Pessoa { NomeFantasia = "Empresa I", CnpjCpf = "90123456789" },
                    new Pessoa { NomeFantasia = "Empresa J", CnpjCpf = "01234567890" }
                );
                context.SaveChanges();
            }
        }
    }
}