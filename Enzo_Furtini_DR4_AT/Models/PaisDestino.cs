using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Models
{
    public class PaisDestino
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome do país é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do país deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(3, MinimumLength = 2, ErrorMessage = "O código do país deve ter entre 2 e 3 caracteres")]
        public string Codigo { get; set; } = string.Empty;
        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        
        // Propriedade de navegação
        public virtual ICollection<CidadeDestino> Cidades { get; set; } = new List<CidadeDestino>();
    }
} 