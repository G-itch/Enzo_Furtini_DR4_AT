using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Models
{
    public class CidadeDestino
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome da cidade é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da cidade deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descricao { get; set; }
        
        public int PaisDestinoId { get; set; }
        public virtual PaisDestino PaisDestino { get; set; } = null!;
        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        
        // Propriedade de navegação para pacotes turísticos
        public virtual ICollection<PacoteTuristico> PacotesTuristicos { get; set; } = new List<PacoteTuristico>();
    }
} 