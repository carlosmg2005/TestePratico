using System.ComponentModel.DataAnnotations;

namespace TestePratico.Models
{
    // Representa uma entidade 'Pessoa' no sistema
    public class Pessoa
    {
        // Chave primária da entidade 'Pessoa'
        [Key]
        public int PessoaId { get; set; }

        // Nome fantasia da pessoa, campo obrigatório com um comprimento máximo de 255 caracteres
        [Required]
        [MaxLength(255)]
        public string NomeFantasia { get; set; }

        // CNPJ ou CPF da pessoa, campo obrigatório com um comprimento máximo de 20 caracteres
        [Required]
        [MaxLength(20)]
        public string CnpjCpf { get; set; }
    }
}
