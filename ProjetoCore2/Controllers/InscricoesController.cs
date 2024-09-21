using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using ProjetoCore2.Data;
using ProjetoCore2.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProjetoCore2.Controllers
{
    public class InscricoesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly ApplicationDbContext _context;

        public InscricoesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Inscricao.Include(i => i.Pessoa);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook()
        {
            try
            {
                string payload;
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    payload = await reader.ReadToEndAsync();
                }

                if (payload.Contains("rejected"))
                {

                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Lide com possíveis erros ou exceções aqui
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IActionResult> Success()
        {
            MercadoPagoConfig.AccessToken = "APP_USR-5683985862198407-102313-eef06eed6ff3e3406435a26ad1e45521-1510639425";
            var modelos = HttpContext.Session.GetString("Modelos");
            var modelosList = JsonConvert.DeserializeObject<List<Modelo>>(modelos, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = "Inscrição GPPSD",
                        Quantity = 1,
                        CurrencyId = "BRL",
                        UnitPrice = modelosList.Count > 4 ? 40 + (modelosList.Count - 4) * 10 : 40,
                        //UnitPrice = 2,
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = Url.Action("Feedback", "Inscricoes", null, Request.Scheme),
                    Failure = Url.Action("Feedback", "Inscricoes", null, Request.Scheme),
                    Pending = Url.Action("Feedback", "Inscricoes", null, Request.Scheme)
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            ViewData["PreferenceId"] = preference.Id;
            return View(modelosList);
        }

        public async Task<IActionResult> Feedback(int Length, string collection_id, string collection_status, string payment_id, string status, string external_reference, string payment_type, string merchant_order_id, string preference_id, string site_id, string processing_mode, string merchant_account_id)
        {
            var modelos = HttpContext.Session.GetString("Modelos");
            var modelosList = JsonConvert.DeserializeObject<List<Modelo>>(modelos, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (status.Equals("approved"))
            {
                var inscricao = _context.Inscricao.FirstOrDefault(x => x.Id == modelosList.Select(x => x.InscricaoId).FirstOrDefault());
                inscricao.Pessoa = _context.Pessoa.FirstOrDefault(x => x.Id == inscricao.PessoaId);
                inscricao.Modelos = modelosList;
                GerarEtiquetasEEnviarPorEmail(inscricao);
                ViewBag.Pendente = "N";
                return View(modelosList);
            }
            else if (status.Equals("rejected"))
            {
                var inscricao = _context.Inscricao.FirstOrDefault(x => x.Id == modelosList.Select(x => x.InscricaoId).FirstOrDefault());
                inscricao.Pessoa = _context.Pessoa.FirstOrDefault(x => x.Id == inscricao.PessoaId);

                _context.RemoveRange(inscricao.Modelos);
                _context.Remove(inscricao.Pessoa);
                _context.Remove(inscricao);

                ViewBag.Pendente = "N";
                return RedirectToAction("PaymentError");
            }
            else if (status == null)
            {
                return RedirectToAction("Success");
            }
            else if (status == "pending")
            {
                ViewBag.Pendente = "S";
                return View(modelosList);
            }
            return View();
        }

        public IActionResult Create()
        {
            ViewData["PessoaId"] = new SelectList(_context.Set<Pessoa>(), "Id", "Id");
            ViewData["TipoId"] = new SelectList(_context.Set<Tipo>(), "Id", "Id");
            ViewData["EscalaId"] = new SelectList(_context.Set<Escala>(), "Id", "Id");
            ViewBag.NivelId = new SelectList(_context.Nivel.OrderBy(x => x.Descricao), "Id", "Descricao");
            ViewBag.Tipos = new SelectList(_context.Tipo.OrderBy(x => x.Descricao), "Id", "Descricao");
            ViewBag.Categorias = new SelectList(_context.Categoria.OrderBy(x => x.Descricao), "Id", "Descricao");
            ViewBag.SubCategorias = new SelectList(_context.SubCategoria.OrderBy(x => x.Descricao), "Id", "Descricao");
            ViewBag.Escalas = new SelectList(_context.Escala.OrderBy(x => x.Descricao), "Id", "Descricao");
            ViewBag.EscalasJson = JsonConvert.SerializeObject(ViewBag.Escalas);
            ViewBag.TiposJson = JsonConvert.SerializeObject(ViewBag.Tipos);
            ViewBag.CategoriasJson = JsonConvert.SerializeObject(ViewBag.Categorias);
            ViewBag.SubCategoriasJson = JsonConvert.SerializeObject(ViewBag.SubCategorias);
            return View();
        }

        public IActionResult PaymentError()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscricao inscricao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Montar uma resposta JSON com os erros de validação
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                    // Verificar se algum dos valores contém a mensagem indesejada e substituir
                    foreach (var entry in errors)
                    {
                        if (entry.Value.Contains("The value '' is invalid."))
                        {
                            entry.Value.RemoveAll(msg => msg == "The value '' is invalid.");
                            entry.Value.Add("Opção selecionada inválida ou não selecionada");
                        }
                    }

                    return Json(new { isValid = false, errors });
                }

                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Add(inscricao.Pessoa);
                    await _context.SaveChangesAsync();

                    foreach (var modelo in inscricao.Modelos)
                    {
                        modelo.SubCategoria = null;
                    }

                    _context.Add(inscricao);
                    await _context.SaveChangesAsync();

                    foreach (var modelo in inscricao.Modelos)
                    {
                        modelo.InscricaoId = inscricao.Id;
                        _context.Update(modelo);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    HttpContext.Session.SetString("Modelos", JsonConvert.SerializeObject(inscricao.Modelos,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

                    return Json(new { isValid = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _context.Dispose();
                    throw new ArgumentException("Não foi possível salvar a inscrição, tente novamente mais tarde.", ex);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ocorreu um erro ao salvar a inscrição.", ex);
            }
        }

        [HttpPost]
        public List<SubCategoria> GetSubCategorias(int categoriaId)
        {
            return _context.SubCategoria.Where(x => x.CategoriaId == categoriaId).ToList();
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

        public void GerarEtiquetasEEnviarPorEmail(Inscricao inscricao)
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
                    if (gfx != null)
                    {
                        gfx.Dispose(); // Descartar o objeto XGraphics anterior
                    }
                    // Criar uma nova página quando a contagem de modelos exceder o limite
                    page = ObterOuCriarPagina(document, modelsOnPage);
                    gfx = XGraphics.FromPdfPage(page);
                    y = 20;
                    modelsOnPage = 0;
                }

                if (modelsOnPage % 2 == 0)
                {
                    AdicionarInformacoesEtiqueta(gfx, inscricao.Pessoa, modelo, xLeftColumn, y); // Posicionar na coluna da esquerda
                }
                else
                {
                    AdicionarInformacoesEtiqueta(gfx, inscricao.Pessoa, modelo, xRightColumn, y); // Posicionar na coluna da direita
                    y += 230; // Incrementar a posição vertical apenas para a coluna da direita
                }

                modelsOnPage++;
            }

            string fileName = "etiquetas.pdf";

            document.Save(fileName);

            foreach (var modelo in inscricao.Modelos)
            {
                modelo.Filipeta = ConvertPdfToBase64(document);
                _context.Update(modelo);
            }

            _context.SaveChangesAsync();

            EnviarEmailComAnexo(document, fileName, inscricao.Pessoa.Email);
        }

        private void AdicionarInformacoesEtiqueta(XGraphics gfx, Pessoa pessoa, Modelo modelo, int x, int y)
        {
            const int width = 200;
            const int height = 225;
            const int padding = 20; // Espaço entre as colunas

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
            // Carregar a imagem da marca d'água
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
            var secondColumnX = x + width + padding * 2;

            gfx.DrawImage(logotipo, firstColumnX, y + padding, 50, 35); // Posicionamento do logotipo

            gfx.DrawString($"ID do Modelista: {pessoa.Id}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 58);
            gfx.DrawString($"Fabricante: {LimitarCaracteres(modelo.Fabricante, 25)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 78);
            //gfx.DrawString($"Nome: {LimitarCaracteres(pessoa.Nome, 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 95);
            gfx.DrawString($"Modelo: {LimitarCaracteres(modelo.Descricao, 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 98);
            gfx.DrawString($"Categoria: {LimitarCaracteres(categoria.Descricao, 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 118);
            gfx.DrawString($"Sub-Categoria: {LimitarCaracteres(subCategoria.Descricao, 19)}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 138);
            gfx.DrawString($"Escala: {escala.Descricao}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 158);
            gfx.DrawString($"ID do modelo: {modelo.Id}", new XFont("Arial", 12), XBrushes.Black, firstColumnX, y + padding + 178);
        }

        private static string LimitarCaracteres(string texto, int limite)
        {
            if (texto.Length > limite)
            {
                return texto[..limite] + "...";
            }
            return texto;
        }

        private static void EnviarEmailComAnexo(PdfDocument document, string fileName, string email)
        {
            var message = new MailMessage
            {
                From = new MailAddress("eventogppsd@hotmail.com")
            };
            message.To.Add(new MailAddress(email));
            message.Subject = $"GPPSD - Inscrição realizada com sucesso!";

            message.Body = $"Olá,\n\nSegue em anexo a etiqueta dos modelos cadastrados.\n\nAgradecemos por se inscrever! É imprescindível que você leve as etiquetas impressas para o evento.";

            using var memoryStream = new MemoryStream();
            document.Save(memoryStream);
            memoryStream.Position = 0;

            message.Attachments.Add(new Attachment(memoryStream, fileName));

            using var client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("eventogppsd@hotmail.com", "Poker2024");

            client.Send(message);
        }
    }
}
