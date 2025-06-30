using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Enzo_Furtini_DR4_AT.Data;
using Enzo_Furtini_DR4_AT.Models;

namespace Enzo_Furtini_DR4_AT.Pages
{
    public class CidadeDetailsModel : PageModel
    {
        private readonly EnzoFurtiniDR4ATContext _context;

        public CidadeDetailsModel(EnzoFurtiniDR4ATContext context)
        {
            _context = context;
        }

        public CidadeDestino? Cidade { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cidade = await _context.CidadesDestino
                .Include(c => c.PaisDestino)
                .Include(c => c.PacotesTuristicos)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (Cidade == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 