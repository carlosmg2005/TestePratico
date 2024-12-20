namespace TestePratico.Models
{
    // Modelo para representar informações de erro
    public class ErrorViewModel
    {
        // Mensagem de erro a ser exibida
        public string Message { get; set; }

        // Exceção associada ao erro, utilizada para captura de detalhes adicionais
        public Exception Exception { get; set; }
    }
}
