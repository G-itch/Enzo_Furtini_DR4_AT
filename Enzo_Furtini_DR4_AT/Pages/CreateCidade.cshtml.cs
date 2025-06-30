using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enzo_Furtini_DR4_AT.Data;
using Enzo_Furtini_DR4_AT.Models;
using Enzo_Furtini_DR4_AT.Services;

namespace Enzo_Furtini_DR4_AT.Pages
{
    public class CreateCidadeModel : PageModel
    {
        private readonly EnzoFurtiniDR4ATContext _context;
        private readonly DelegatesService _delegatesService;

        public CreateCidadeModel(EnzoFurtiniDR4ATContext context, DelegatesService delegatesService)
        {
            _context = context;
            _delegatesService = delegatesService;
        }

        [BindProperty]
        public CidadeDestino Cidade { get; set; } = new();

        public SelectList PaisesOptions { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPaisesOptions();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadPaisesOptions();
                return Page();
            }

            _context.CidadesDestino.Add(Cidade);
            await _context.SaveChangesAsync();

            // Registra a operação usando o delegate
            _delegatesService.RegistrarOperacao($"Nova cidade cadastrada: {Cidade.Nome}");

            return RedirectToPage("/Cidades/Index");
        }

        private async Task LoadPaisesOptions()
        {
            var paises = await _context.PaisesDestino
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.Nome)
                .ToListAsync();

            PaisesOptions = new SelectList(paises, "Id", "Nome");
        }
    }
} 