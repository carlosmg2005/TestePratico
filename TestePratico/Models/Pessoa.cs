using System.ComponentModel.DataAnnotations;

namespace TestePratico.Models
{
    public class Pessoa
    {
        [Key]
        public int PessoaId { get; set; }

        [Required]
        [MaxLength(255)]
        public string NomeFantasia { get; set; }

        [Required]
        [MaxLength(20)]
        public string CnpjCpf { get; set; }
    }
}