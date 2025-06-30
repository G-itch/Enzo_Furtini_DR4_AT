using Enzo_Furtini_DR4_AT.Models;

namespace Enzo_Furtini_DR4_AT.Services
{
    // Delegate para cálculo de descontos
    public delegate decimal CalculateDiscountDelegate(decimal preco, decimal percentualDesconto);
    
    // Delegate para logs
    public delegate void LogDelegate(string mensagem);
    
    public class DelegatesService
    {
        private readonly LogDelegate _logDelegate;
        private readonly CalculateDiscountDelegate _calculateDiscountDelegate;
        
        public DelegatesService()
        {
            // Inicializa o multicast delegate para logs
            _logDelegate = LogToConsole;
            _logDelegate += LogToFile;
            _logDelegate += LogToMemory;
            
            // Inicializa o delegate para cálculo de desconto
            _calculateDiscountDelegate = CalculateDiscount;
        }
        
        // 1. Delegate para cálculo de desconto de 10%
        public decimal CalculateDiscount(decimal preco, decimal percentualDesconto = 10)
        {
            return preco * (1 - percentualDesconto / 100);
        }
        
        // 2. Métodos para multicast delegate de logs
        public void LogToConsole(string mensagem)
        {
            Console.WriteLine($"[CONSOLE] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}");
        }
        
        public void LogToFile(string mensagem)
        {
            var logPath = Path.Combine("wwwroot", "files", "system.log");
            var logDir = Path.GetDirectoryName(logPath);
            
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir!);
                
            File.AppendAllText(logPath, $"[FILE] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}{Environment.NewLine}");
        }
        
        public void LogToMemory(string mensagem)
        {
            // Simula log em memória (em uma aplicação real, seria uma lista ou cache)
            Console.WriteLine($"[MEMORY] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}");
        }
        
        // 3. Func com expressão lambda para cálculo de valor total
        public Func<int, int, decimal> CalculateTotalValue = (numeroDiarias, valorDiaria) => 
            numeroDiarias * valorDiaria;
        
        // 4. Método para registrar operações do sistema
        public void RegistrarOperacao(string operacao)
        {
            _logDelegate($"Operação realizada: {operacao}");
        }
        
        // 5. Método para calcular desconto usando o delegate
        public decimal AplicarDesconto(decimal preco, decimal percentualDesconto = 10)
        {
            return _calculateDiscountDelegate(preco, percentualDesconto);
        }
        
        // 6. Evento para quando a capacidade for atingida
        public void OnCapacityReached(object? sender, CapacityReachedEventArgs e)
        {
            var mensagem = $"ALERTA: Capacidade máxima atingida para o pacote '{e.Titulo}' (ID: {e.PacoteId}). " +
                          $"Capacidade: {e.CapacidadeMaxima}, Reservas atuais: {e.ReservasAtuais}";
            
            _logDelegate(mensagem);
        }
    }
} 