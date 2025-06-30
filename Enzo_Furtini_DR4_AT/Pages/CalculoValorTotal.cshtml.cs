using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enzo_Furtini_DR4_AT.Services;
using System.ComponentModel.DataAnnotations;

namespace Enzo_Furtini_DR4_AT.Pages
{
    public class CalculoValorTotalModel : PageModel
    {
        private readonly DelegatesService _delegatesService;

        public CalculoValorTotalModel(DelegatesService delegatesService)
        {
            _delegatesService = delegatesService;
        }

        [BindProperty]
        [Required(ErrorMessage = "O número de diárias é obrigatório")]
        [Range(1, 30, ErrorMessage = "O número de diárias deve estar entre 1 e 30")]
        public int NumeroDiarias { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "O valor da diária é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da diária deve ser maior que zero")]
        public decimal ValorDiaria { get; set; }

        public decimal ValorTotal { get; set; }
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

            // Usa o Func com expressão lambda para calcular o valor total
            ValorTotal = _delegatesService.CalculateTotalValue(NumeroDiarias, (int)ValorDiaria);
            ResultadoCalculado = true;

            // Registra a operação
            _delegatesService.RegistrarOperacao($"Cálculo de valor total realizado: {NumeroDiarias} diárias x R${ValorDiaria:F2} = R${ValorTotal:F2}");

            return Page();
        }
    }
} 