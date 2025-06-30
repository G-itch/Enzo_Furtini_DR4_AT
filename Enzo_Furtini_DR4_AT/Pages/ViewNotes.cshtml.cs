using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Enzo_Furtini_DR4_AT.Services;
using System.IO;

namespace Enzo_Furtini_DR4_AT.Pages
{
    public class ViewNotesModel : PageModel
    {
        private readonly DelegatesService _delegatesService;

        public ViewNotesModel(DelegatesService delegatesService)
        {
            _delegatesService = delegatesService;
        }

        [BindProperty]
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 2000 caracteres")]
        public string Conteudo { get; set; } = string.Empty;

        public List<ArquivoNota> ArquivosNotas { get; set; } = new();
        public string? ConteudoSelecionado { get; set; }
        public string? ArquivoSelecionado { get; set; }
        public string? Mensagem { get; set; }

        public void OnGet(string? arquivo)
        {
            CarregarArquivosNotas();
            
            if (!string.IsNullOrEmpty(arquivo))
            {
                VisualizarArquivo(arquivo);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarArquivosNotas();
                return Page();
            }

            SalvarNota();
            CarregarArquivosNotas();

            return Page();
        }

        private void SalvarNota()
        {
            try
            {
                var diretorio = Path.Combine("wwwroot", "files");
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                var nomeArquivo = $"{Titulo.Replace(" ", "_").Replace("/", "_").Replace("\\", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var caminhoArquivo = Path.Combine(diretorio, nomeArquivo);

                var conteudoCompleto = $"Título: {Titulo}\nData: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n\n{Conteudo}";
                System.IO.File.WriteAllText(caminhoArquivo, conteudoCompleto);

                Mensagem = $"Nota '{Titulo}' salva com sucesso!";
                
                // Registra a operação usando o delegate
                _delegatesService.RegistrarOperacao($"Nova nota criada: {Titulo}");

                // Limpa os campos após salvar
                Titulo = string.Empty;
                Conteudo = string.Empty;
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao salvar a nota: {ex.Message}";
            }
        }

        private void CarregarArquivosNotas()
        {
            try
            {
                var diretorio = Path.Combine("wwwroot", "files");
                if (Directory.Exists(diretorio))
                {
                    var arquivos = Directory.GetFiles(diretorio, "*.txt")
                        .Select(f => new ArquivoNota
                        {
                            Nome = Path.GetFileName(f),
                            Caminho = f,
                            DataCriacao = System.IO.File.GetCreationTime(f)
                        })
                        .OrderByDescending(a => a.DataCriacao)
                        .ToList();

                    ArquivosNotas = arquivos;
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao carregar arquivos: {ex.Message}";
            }
        }

        private void VisualizarArquivo(string nomeArquivo)
        {
            try
            {
                var caminhoArquivo = Path.Combine("wwwroot", "files", nomeArquivo);
                if (System.IO.File.Exists(caminhoArquivo))
                {
                    ConteudoSelecionado = System.IO.File.ReadAllText(caminhoArquivo);
                    ArquivoSelecionado = nomeArquivo;
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao visualizar arquivo: {ex.Message}";
            }
        }
    }

    public class ArquivoNota
    {
        public string Nome { get; set; } = string.Empty;
        public string Caminho { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
} 