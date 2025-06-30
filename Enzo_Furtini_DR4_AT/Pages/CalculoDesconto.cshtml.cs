using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enzo_Furtini_DR4_AT.Services;
using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Pages
{
    public class CalculoDescontoModel : PageModel
    {
        private readonly DelegatesService _delegatesService;

        public CalculoDescontoModel(DelegatesService delegatesService)
        {
            _delegatesService = delegatesService;
        }

        [BindProperty]
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [BindProperty]
        [Range(0, 100, ErrorMessage = "O percentual deve estar entre 0 e 100")]
        public decimal PercentualDesconto { get; set; } = 10;

        public decimal PrecoComDesconto { get; set; }
        public decimal Economia { get; set; }
        public bool ResultadoCalculado { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Usa o delegate para calcular o desconto
            PrecoComDesconto = _delegatesService.AplicarDesconto(Preco, PercentualDesconto);
            Economia = Preco - PrecoComDesconto;
            ResultadoCalculado = true;

            // Registra a operação usando o multicast delegate
            _delegatesService.RegistrarOperacao($"Cálculo de desconto realizado: Preço R${Preco:F2}, Desconto {PercentualDesconto}%, Resultado R${PrecoComDesconto:F2}");

            return Page();
        }
    }
} 