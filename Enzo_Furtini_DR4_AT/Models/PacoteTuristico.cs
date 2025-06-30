using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
        public string Descricao { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A data de início é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }
        
        [Required(ErrorMessage = "A data de fim é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }
        
        [Required(ErrorMessage = "A capacidade máxima é obrigatória")]
        [Range(1, 100, ErrorMessage = "A capacidade deve estar entre 1 e 100")]
        public int CapacidadeMaxima { get; set; }
        
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        
        // Propriedades de navegação
        public virtual ICollection<CidadeDestino> Destinos { get; set; } = new List<CidadeDestino>();
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        
        // Evento para quando a capacidade for atingida
        public event EventHandler<CapacityReachedEventArgs>? CapacityReached;
        
        // Método para disparar o evento
        protected virtual void OnCapacityReached(int reservasAtuais)
        {
            CapacityReached?.Invoke(this, new CapacityReachedEventArgs
            {
                PacoteId = Id,
                Titulo = Titulo,
                CapacidadeMaxima = CapacidadeMaxima,
                ReservasAtuais = reservasAtuais
            });
        }
        
        // Método para verificar se a capacidade foi atingida
        public void VerificarCapacidade()
        {
            if (Reservas.Count >= CapacidadeMaxima)
            {
                OnCapacityReached(Reservas.Count);
            }
        }
    }
    
    public class CapacityReachedEventArgs : EventArgs
    {
        public int PacoteId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
        public int ReservasAtuais { get; set; }
    }
} 