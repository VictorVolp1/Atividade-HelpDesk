using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDeskMvc.Data;
using HelpDeskMvc.Models;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var chamados = await _context.Chamados.OrderByDescending(c => c.DataAbertura).ToListAsync();
            return View(chamados);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CriarChamadoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var chamado = new Chamado
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Status = "Aberto",
                DataAbertura = DateTime.Now,
                DataFechamento = null
            };

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("api/chamados")]
        public async Task<IActionResult> CriarViaApi([FromBody] CriarChamadoViewModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var chamado = new Chamado
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Status = "Aberto",
                DataAbertura = DateTime.Now,
                DataFechamento = null
            };

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Detalhes), new { id = chamado.Id }, chamado);
        }
    }
}