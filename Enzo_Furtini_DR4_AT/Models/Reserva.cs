using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; } = null!;
        
        public int PacoteTuristicoId { get; set; }
        public virtual PacoteTuristico PacoteTuristico { get; set; } = null!;
        
        [Required(ErrorMessage = "A data da reserva é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataReserva { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "O número de participantes é obrigatório")]
        [Range(1, 10, ErrorMessage = "O número de participantes deve estar entre 1 e 10")]
        public int NumeroParticipantes { get; set; }
        
        [Required(ErrorMessage = "O valor total é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero")]
        public decimal ValorTotal { get; set; }
        
        [StringLength(500)]
        public string? Observacoes { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
} 