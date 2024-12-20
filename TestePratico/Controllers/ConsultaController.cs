using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using TestePratico.Models;
using TestePratico.Data;
using Microsoft.EntityFrameworkCore;

namespace TestePratico.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var pessoas = _context.Pessoas.ToList();
                return View(pessoas);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    Message = "Ocorreu um erro ao carregar a lista de pessoas.",
                    Exception = ex
                });
            }
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _context.Pessoas.Add(pessoa);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }


        public IActionResult Detalhe(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        [HttpPost]
        public IActionResult Detalhe(Pessoa pessoa)
        {
            if (!ModelState.IsValid)
            {
                return View(pessoa);
            }

            // Buscar a entidade original no banco
            var pessoaDb = _context.Pessoas.Find(pessoa.PessoaId);
            if (pessoaDb == null)
            {
                return NotFound();
            }

            // Atualizar apenas os campos permitidos
            pessoaDb.NomeFantasia = pessoa.NomeFantasia;

            try
            {
                _context.SaveChanges();
                ViewBag.Message = "Alterações salvas com sucesso!";
                return View(pessoaDb); // Retorna a mesma view com a mensagem de sucesso
            }
            catch (Exception ex)
            {
                // Tratar possíveis exceções durante o salvamento
                return View("Error", new ErrorViewModel
                {
                    Message = "Erro ao salvar as alterações.",
                    Exception = ex
                });
            }
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            try
            {
                _context.Pessoas.Remove(pessoa);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Pessoa excluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    Message = "Erro ao excluir a pessoa.",
                    Exception = ex
                });
            }
        }
    }
}