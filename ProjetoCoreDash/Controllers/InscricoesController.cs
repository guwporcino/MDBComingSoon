using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using ProjetoCoreDash.Data;
using ProjetoCoreDash.Models;

namespace ProjetoCoreDash.Controllers
{
    public class InscricoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InscricoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString = "", int? searchIdModelista = 0, int? searchIdModelo = 0, int? searchAnoInscricao = null, string searchTipoEvento = "")
        {
            string referencia = null;
            string tipoEvento = null;

            if (searchAnoInscricao is not null)
            {
                referencia = searchAnoInscricao.ToString();
            }

            if (searchTipoEvento is not null)
            {
                tipoEvento = searchTipoEvento.ToString();
            }

            if (string.IsNullOrEmpty(searchString) && searchIdModelo == 0)
            {
                List<InscricaoViewModel> Inscricoes = new();
                return View(Inscricoes);
            }
            else if (searchIdModelo is not null)
            {
                List<InscricaoViewModel> modelos = null;

                if (!string.IsNullOrEmpty(referencia) && !string.IsNullOrEmpty(tipoEvento))
                {
                    string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                    var teste = _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Modelos.Any(y => y.Id == searchIdModelo) && x.AnoInscricao == referencia && x.TipoEvento == charEvento)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .AsQueryable();

                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Modelos.Any(y => y.Id == searchIdModelo) && x.AnoInscricao == referencia && x.TipoEvento == charEvento)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                }
                else if (!string.IsNullOrEmpty(referencia))
                {
                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Modelos.Any(y => y.Id == searchIdModelo) && x.AnoInscricao == referencia)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                } else if (!string.IsNullOrEmpty(tipoEvento))
                {
                    string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Modelos.Any(y => y.Id == searchIdModelo) && x.TipoEvento == charEvento)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                } else
                {
                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Modelos.Any(y => y.Id == searchIdModelo) && x.AnoInscricao == referencia)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                }

                if (modelos.Count < 1)
                {
                    List<InscricaoViewModel> Inscricoes = new();
                    TempData["ErrorMessage"] = "Nenhuma inscrição encontrada para os parâmetros informados. Refaça a pesquisa.";
                    return View(Inscricoes);
                }

                return View(modelos);
            }
            else if (searchIdModelista is not null)
            {
                List<InscricaoViewModel> modelos = null;

                if (!string.IsNullOrEmpty(referencia) && !string.IsNullOrEmpty(tipoEvento))
                {
                    string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Pessoa.Id == searchIdModelista && x.AnoInscricao == referencia && x.TipoEvento == charEvento)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                }
                else if (!string.IsNullOrEmpty(referencia))
                {
                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Pessoa.Id == searchIdModelista && x.AnoInscricao == referencia)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                }
                else if (!string.IsNullOrEmpty(tipoEvento))
                {
                    string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Pessoa.Id == searchIdModelista && x.TipoEvento == charEvento)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                } else
                {
                    modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Pessoa.Id == searchIdModelista)
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                }

                if (modelos.Count < 1)
                {
                    List<InscricaoViewModel> Inscricoes = new();
                    TempData["ErrorMessage"] = "Nenhuma inscrição encontrada para os parâmetros informados. Refaça a pesquisa.";
                    return View(Inscricoes);
                }

                return View(modelos);
            }
            else
            {
                if (!string.IsNullOrEmpty(searchString) && searchString.Length < 3)
                {
                    List<Inscricao> Inscricoes = new();
                    TempData["ErrorMessage"] = "Nenhuma inscrição encontrada para os parâmetros informados. Refaça a pesquisa.";
                    return View(Inscricoes);
                }
                else if (string.IsNullOrEmpty(searchString))
                {
                    List<Inscricao> Inscricoes = new();
                    TempData["ErrorMessage"] = "Nenhuma inscrição encontrada para os parâmetros informados. Refaça a pesquisa.";
                    return View(Inscricoes);
                }
                else
                {
                    List<InscricaoViewModel> modelos = null;

                    if (!string.IsNullOrEmpty(referencia) && !string.IsNullOrEmpty(tipoEvento))
                    {
                        string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                        modelos = await _context.Inscricao
                            .AsNoTracking()
                            .AsSplitQuery()
                            .Where(x => (x.Pessoa.Nome.StartsWith(searchString.ToUpperInvariant()) || x.Pessoa.Nome.EndsWith(searchString.ToUpperInvariant())) && x.AnoInscricao == referencia && x.TipoEvento == charEvento)
                            .Include(i => i.Modelos)
                            .Select(i => new InscricaoViewModel
                            {
                                IdInscricao = i.Id,
                                Nome = i.Pessoa.Nome,
                                QuantidadeModelos = i.Modelos.Count()
                            })
                            .ToListAsync();
                    }
                    else if (!string.IsNullOrEmpty(referencia))
                    {
                        modelos = await _context.Inscricao
                            .AsNoTracking()
                            .AsSplitQuery()
                            .Where(x => x.Pessoa.Nome.StartsWith(searchString.ToUpperInvariant()) || x.Pessoa.Nome.EndsWith(searchString.ToUpperInvariant()) && x.AnoInscricao == referencia)
                            .Include(i => i.Modelos)
                            .Select(i => new InscricaoViewModel
                            {
                                IdInscricao = i.Id,
                                Nome = i.Pessoa.Nome,
                                QuantidadeModelos = i.Modelos.Count()
                            })
                            .ToListAsync();
                    }
                    else if (!string.IsNullOrEmpty(tipoEvento))
                    {
                        string charEvento = tipoEvento == "Convenção" ? "C" : "P";

                        modelos = await _context.Inscricao
                            .AsNoTracking()
                            .AsSplitQuery()
                            .Where(x => x.Pessoa.Nome.StartsWith(searchString.ToUpperInvariant()) || x.Pessoa.Nome.EndsWith(searchString.ToUpperInvariant()) && x.TipoEvento == charEvento)
                            .Include(i => i.Modelos)
                            .Select(i => new InscricaoViewModel
                            {
                                IdInscricao = i.Id,
                                Nome = i.Pessoa.Nome,
                                QuantidadeModelos = i.Modelos.Count()
                            })
                            .ToListAsync();
                    } else
                    {
                        modelos = await _context.Inscricao
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(x => x.Pessoa.Nome.StartsWith(searchString.ToUpperInvariant()) || x.Pessoa.Nome.EndsWith(searchString.ToUpperInvariant()))
                        .Include(i => i.Modelos)
                        .Select(i => new InscricaoViewModel
                        {
                            IdInscricao = i.Id,
                            Nome = i.Pessoa.Nome,
                            QuantidadeModelos = i.Modelos.Count()
                        })
                        .ToListAsync();
                    }

                    if (modelos.Count < 1)
                    {
                        List<InscricaoViewModel> Inscricoes = new();
                        TempData["ErrorMessage"] = "Nenhuma inscrição encontrada para os parâmetros informados. Refaça a pesquisa.";
                        return View(Inscricoes);
                    }

                    return View(modelos);
                }
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inscricao == null)
            {
                TempData["ErrorMessage"] = "Inscrição não encontrada no banco de dados.";
                return RedirectToAction("Index");
            }

            var inscricao = await _context.Inscricao
                .AsNoTracking()
                .AsSplitQuery()
                .Include(i => i.Pessoa)
                .ThenInclude(i => i.Nivel)
                .Include(i => i.Modelos)
                .ThenInclude(m => m.Tipo)
                .Include(i => i.Modelos)
                .ThenInclude(m => m.Escala)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inscricao == null)
            {
                TempData["ErrorMessage"] = "Inscrição não encontrada no banco de dados.";
                return RedirectToAction("Index");
            }

            return View(inscricao);
        }

        [AllowAnonymous]
        public IActionResult BaixarFilipeta(int modeloId)
        {
            var modelo = _context.Modelo.FirstOrDefault(x => x.Id == modeloId);
            Inscricao inscricao = null;

            if (string.IsNullOrEmpty(modelo.Filipeta))
            {
                try
                {
                    inscricao = _context.Inscricao.AsNoTracking()
                                .Include(i => i.Pessoa)
                                .Include(i => i.Modelos)
                                .FirstOrDefault(x => x.Id == modelo.InscricaoId);

                    GerarEtiquetas(inscricao);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Erro capturado: {e.Message}, StackTrace: {e.StackTrace}");
                    TempData["ErrorMessage"] = $"Não foi possível baixar/gerar a filipeta para este modelo. Erro: {e.Message}, StackTrace: {e.StackTrace}";
                    return RedirectToAction("Details", new { id = modelo.InscricaoId });
                }
            }

            var modeloResult = inscricao.Modelos.FirstOrDefault(x => x.Id == modeloId) ?? throw new InvalidOperationException($"Modelo com o ID {modeloId} não foi encontrado.");

            if (string.IsNullOrEmpty(modeloResult.Filipeta))
            {
                // Retorna um erro ou lança uma exceção informando que a Filipeta está vazia
                throw new InvalidOperationException("A Filipeta está vazia ou não foi gerada.");
            }

            var pdfBytes = Convert.FromBase64String(modeloResult.Filipeta);

            return File(pdfBytes, "application/pdf", $"etiqueta_{modelo.Descricao}.pdf");
        }

        private static PdfPage ObterOuCriarPagina(PdfDocument document, int modelsOnPage)
        {
            if (document.PageCount > 0 && modelsOnPage < 4)
            {
                int lastPageIndex = document.PageCount - 1;
                return document.Pages[lastPageIndex];
            }

            return document.AddPage();
        }

        private static bool PaginaEstaCheia(PdfPage page)
        {
            const int etiquetasPorPagina = 4;

            return page.Elements.Count >= etiquetasPorPagina;
        }

        private static PdfPage CriarNovaPagina(PdfDocument document)
        {
            return document.AddPage();
        }

        private static string ConvertPdfToBase64(PdfDocument document)
        {
            using var memoryStream = new MemoryStream();
            document.Save(memoryStream);
            memoryStream.Position = 0;

            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        public void GerarEtiquetas(Inscricao inscricao)
        {
            var document = new PdfDocument();
            PdfPage page = null;
            XGraphics gfx = null;

            const int maxModelsPerPage = 6;
            int modelsOnPage = 0;
            const int xLeftColumn = 50;
            const int xRightColumn = 300;
            int y = 20;

            foreach (var modelo in inscricao.Modelos)
            {
                if (page == null || modelsOnPage >= maxModelsPerPage)
                {
                    gfx?.Dispose();
                    page = ObterOuCriarPagina(document, modelsOnPage);
                    gfx = XGraphics.FromPdfPage(page);
                    y = 20;
                    modelsOnPage = 0;
                }

                if (modelsOnPage % 2 == 0)
                {
                    AdicionarInformacoesEtiqueta(gfx, inscricao.Pessoa, modelo, xLeftColumn, y);
                }
                else
                {
                    AdicionarInformacoesEtiqueta(gfx, inscricao.Pessoa, modelo, xRightColumn, y);
                    y += 230;
                }

                modelsOnPage++;
            }

            string fileName = "etiquetas.pdf";

            document.Save(fileName);

            foreach (var modelo in inscricao.Modelos)
            {
                var existingModelo = _context.Modelo.Local.FirstOrDefault(m => m.Id == modelo.Id);
                if (existingModelo != null)
                {
                    _context.Entry(existingModelo).State = EntityState.Detached;
                }

                modelo.Filipeta = ConvertPdfToBase64(document);
                _context.Update(modelo);
            }

            _context.SaveChangesAsync();
        }

        private void AdicionarInformacoesEtiqueta(XGraphics gfx, Pessoa pessoa, Modelo modelo, int x, int y)
        {
            const int width = 200;
            const int height = 225;
            const int padding = 20;

            var dashedPen = new XPen(XColors.Black, 1)
            {
                DashStyle = XDashStyle.Dash
            };

            gfx.DrawLine(dashedPen, x, y + height, x + (width + padding) * 2, y + height); // Linha tracejada horizontal

            gfx.DrawLine(dashedPen, x + width + padding, y, x + width + padding, y + height); // Linha tracejada vertical no meio das colunas

            gfx.DrawRectangle(XBrushes.White, x, y, width, height); // Retângulo da coluna da esquerda
            gfx.DrawRectangle(XBrushes.White, x + width + padding * 2, y, width, height); // Retângulo da coluna da direita

            var subCategoria = _context.SubCategoria.FirstOrDefault(x => x.Id == modelo.SubCategoriaId);
            var categoria = _context.Categoria.FirstOrDefault(x => x.Id == subCategoria.CategoriaId);
            var escala = _context.Escala.FirstOrDefault(x => x.Id == modelo.EscalaId);

            var logotipo = XImage.FromFile(Path.Combine("wwwroot", "img", "Logo GPPSD CDR setembro 2012[1]_2.png"));
            var watermarkPath = Path.Combine("wwwroot", "img", "marca_dagua_gppsd.png");
            var watermark = XImage.FromFile(watermarkPath);

            // Definir uma cor de fundo transparente
            XSolidBrush transparentBackground = new(XColor.FromArgb(0, 255, 255, 255));

            // Preencher a coluna da esquerda com a cor de fundo transparente
            gfx.DrawRectangle(transparentBackground, x, y, width + padding, height);

            // Desenhar a imagem da marca d'água como plano de fundo na coluna da esquerda
            gfx.DrawImage(watermark, new XRect(x, y, width + padding, height));

            // Preencher a coluna da direita com a cor de fundo transparente
            gfx.DrawRectangle(transparentBackground, x + width + padding * 2, y, width + padding, height);

            // Desenhar a imagem da marca d'água como plano de fundo na coluna da direita
            gfx.DrawImage(watermark, new XRect(x + width + padding * 2, y, width + padding, height));

            var firstColumnX = x + padding;

            // Desenhar logotipo
            gfx.DrawImage(logotipo, firstColumnX, y + padding, 50, 35); // Posicionamento do logotipo

            // Informação do ID do Modelista e outros detalhes
            gfx.DrawString($"ID do Modelista: {pessoa.Id}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 58);
            gfx.DrawString($"Fabricante: {LimitarCaracteres(modelo.Fabricante, 25)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 78);
            gfx.DrawString($"Modelo: {LimitarCaracteres(modelo.Descricao, 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 98);
            gfx.DrawString($"Categoria: {LimitarCaracteres(categoria?.Descricao ?? "N/A", 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 118);
            gfx.DrawString($"Sub-Categoria: {LimitarCaracteres(subCategoria?.Descricao ?? "N/A", 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 138);
            gfx.DrawString($"Escala: {escala?.Descricao ?? "N/A"}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 158);

            // Destaque para o ID do Modelo (última linha)
            var highlightBrush = new XSolidBrush(XColors.White); // Fundo amarelo
            var fontDestaque = new XFont("Arial", 16, XFontStyle.Bold); // Fonte maior e em negrito

            // Preenchimento com fundo amarelo para o ID do Modelo
            gfx.DrawRectangle(highlightBrush, firstColumnX - 5, y + padding + 170, 178, 25); // Ajustar o tamanho do retângulo de acordo com a necessidade

            // Texto do ID do modelo
            gfx.DrawString($"ID do modelo: {modelo.Id}", fontDestaque, XBrushes.Black, firstColumnX, y + padding + 195);
        }

        private static string LimitarCaracteres(string texto, int limite)
        {
            if (texto.Length > limite)
            {
                return texto[..limite] + "...";
            }
            return texto;
        }

        public IActionResult GerarRelatorio()
        {
            // Obter os dados do contexto do Entity Framework
            var dadosInscricoes = _context.Inscricao.AsNoTracking()
                .Include(i => i.Pessoa)
                .ThenInclude(i => i.Nivel)
                .Include(i => i.Modelos)
                .ThenInclude(t => t.SubCategoria)
                .ThenInclude(ic => ic.Categoria)
                .Include(i => i.Modelos)
                .ThenInclude(t => t.Tipo)
                .Include(i => i.Modelos)
                .ThenInclude(t => t.Escala)
                .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatorio");

            // Adicionar cabeçalho das inscrições
            int coluna = 1;
            worksheet.Cell(1, coluna).Value = "Id da Inscrição";
            worksheet.Cell(1, ++coluna).Value = "Nome do Participante";
            worksheet.Cell(1, ++coluna).Value = "E-mail do Participante";
            worksheet.Cell(1, ++coluna).Value = "Cpf do Participante";
            worksheet.Cell(1, ++coluna).Value = "Telefone do Participante";
            worksheet.Cell(1, ++coluna).Value = "Nível do Modelista";

            // Adicionar cabeçalho dos modelos
            worksheet.Cell(1, ++coluna).Value = "Id do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Descrição do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Escala do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Fabricante do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Tipo do Modelo";
            worksheet.Cell(1, ++coluna).Value = "SubCategoria do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Categoria do Modelo";
            worksheet.Cell(1, ++coluna).Value = "Filipeta Gerada";

            // Adicionar dados das inscrições e modelos
            int linha = 2;
            foreach (var inscricao in dadosInscricoes)
            {
                foreach (var modelo in inscricao.Modelos)
                {
                    coluna = 1;
                    worksheet.Cell(linha, coluna).Value = inscricao.Id;
                    worksheet.Cell(linha, ++coluna).Value = inscricao.Pessoa?.Nome;
                    worksheet.Cell(linha, ++coluna).Value = inscricao.Pessoa?.Email;
                    worksheet.Cell(linha, ++coluna).Value = inscricao.Pessoa?.Cpf;
                    worksheet.Cell(linha, ++coluna).Value = inscricao.Pessoa?.Telefone;
                    worksheet.Cell(linha, ++coluna).Value = inscricao.Pessoa?.Nivel?.Descricao;

                    worksheet.Cell(linha, ++coluna).Value = modelo.Id;
                    worksheet.Cell(linha, ++coluna).Value = modelo.Descricao;
                    worksheet.Cell(linha, ++coluna).Value = modelo.Escala?.Descricao;
                    worksheet.Cell(linha, ++coluna).Value = modelo.Fabricante;
                    worksheet.Cell(linha, ++coluna).Value = modelo.Tipo?.Descricao;
                    worksheet.Cell(linha, ++coluna).Value = modelo.SubCategoria?.Descricao;
                    worksheet.Cell(linha, ++coluna).Value = modelo.SubCategoria?.Categoria?.Descricao;
                    worksheet.Cell(linha, ++coluna).Value = modelo.Filipeta is null ? "N" : "S";

                    linha++;
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var relatorioBytes = stream.ToArray();

            // Retornar o relatório para download
            string fileName = $"relatorio_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(relatorioBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private static object ConverterParaTipoExcel(object valor)
        {
            if (valor is null)
                return string.Empty;
            else if (valor is DateTime data)
                return data; // Neste exemplo, consideramos que a propriedade é do tipo DateTime
            else
                return valor.ToString(); // Conversão para string para outros tipos de dados
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inscricao == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricao.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound();
            }
            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id", inscricao.PessoaId);
            return View(inscricao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PessoaId")] Inscricao inscricao)
        {
            if (id != inscricao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscricao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscricaoExists(inscricao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id", inscricao.PessoaId);
            return View(inscricao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inscricao == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricao.AsNoTracking()
                .Include(i => i.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscricao == null)
            {
                return NotFound();
            }

            return View(inscricao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inscricao == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inscricao'  is null.");
            }
            var inscricao = await _context.Inscricao.FindAsync(id);
            if (inscricao != null)
            {
                _context.Inscricao.Remove(inscricao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscricaoExists(int id)
        {
            return _context.Inscricao.AsNoTracking().Any(e => e.Id == id);
        }
    }
}
