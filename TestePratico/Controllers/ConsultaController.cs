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

        // Ação para exibir a lista de pessoas
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

        // Ação para exibir o formulário de criação de pessoa
        public IActionResult Criar()
        {
            return View();
        }

        // Ação para salvar uma nova pessoa
        [HttpPost]
        public IActionResult Criar(Pessoa pessoa)
        {
            if (!ModelState.IsValid)
            {
                return View(pessoa); // Retorna a view caso os dados não sejam válidos
            }

            try
            {
                _context.Pessoas.Add(pessoa);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Pessoa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                return View("Error", new ErrorViewModel
                {
                    Message = "Erro ao salvar no banco de dados.",
                    Exception = ex
                });
            }
        }

        // Ação para exibir os detalhes de uma pessoa específica
        public IActionResult Detalhe(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // Ação para atualizar os detalhes de uma pessoa
        [HttpPost]
        public IActionResult Detalhe(Pessoa pessoa)
        {
            if (!ModelState.IsValid)
            {
                return View(pessoa);
            }

            var pessoaDb = _context.Pessoas.Find(pessoa.PessoaId);
            if (pessoaDb == null)
            {
                return NotFound();
            }

            pessoaDb.NomeFantasia = pessoa.NomeFantasia;
            pessoaDb.CnpjCpf = pessoa.CnpjCpf;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Alterações salvas com sucesso!";
                return RedirectToAction(nameof(Detalhe), new { id = pessoa.PessoaId });
            }
            catch (DbUpdateException ex)
            {
                return View("Error", new ErrorViewModel
                {
                    Message = "Erro ao atualizar no banco de dados.",
                    Exception = ex
                });
            }
        }

        // Ação para excluir uma pessoa
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
            catch (DbUpdateException ex)
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