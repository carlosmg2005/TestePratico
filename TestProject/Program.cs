using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Executa os testes automatizados usando dotnet test
                var result = System.Diagnostics.Process.Start("dotnet", "test");
                result.WaitForExit();

                if (result.ExitCode == 0)
                {
                    Console.WriteLine("Testes concluídos. Todos os testes foram bem-sucedidos.");
                }
                else
                {
                    Console.WriteLine("Alguns testes falharam.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar os testes: {ex.Message}");
            }
        }
    }
}