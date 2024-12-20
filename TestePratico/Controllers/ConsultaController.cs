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
        // Contexto da aplicação para acessar o banco de dados
        private readonly ApplicationDbContext _context;

        // Construtor para inicializar o contexto
        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ação para exibir a lista de pessoas
        public IActionResult Index()
        {
            try
            {
                // Obter todas as pessoas do banco de dados
                var pessoas = _context.Pessoas.ToList();
                return View(pessoas); // Retorna a view com a lista de pessoas
            }
            catch (Exception ex)
            {
                // Exibir uma mensagem de erro em caso de falha
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
            return View(); // Retorna a view de criação
        }

        // Ação para salvar uma nova pessoa
        [HttpPost]
        public IActionResult Criar(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                // Adicionar a nova pessoa ao contexto e salvar no banco
                _context.Pessoas.Add(pessoa);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redirecionar para a página de listagem
            }
            return View(pessoa); // Retornar a view caso os dados não sejam válidos
        }

        // Ação para exibir os detalhes de uma pessoa específica
        public IActionResult Detalhe(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound(); // Retorna 404 se a pessoa não for encontrada
            }
            return View(pessoa); // Retorna a view com os detalhes da pessoa
        }

        // Ação para atualizar os detalhes de uma pessoa
        [HttpPost]
        public IActionResult Detalhe(Pessoa pessoa)
        {
            if (!ModelState.IsValid)
            {
                return View(pessoa); // Retornar a view caso os dados não sejam válidos
            }

            // Buscar a entidade original no banco
            var pessoaDb = _context.Pessoas.Find(pessoa.PessoaId);
            if (pessoaDb == null)
            {
                return NotFound(); // Retorna 404 se a pessoa não for encontrada
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

        // Ação para excluir uma pessoa
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound(); // Retorna 404 se a pessoa não for encontrada
            }

            try
            {
                _context.Pessoas.Remove(pessoa);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Pessoa excluída com sucesso!";
                return RedirectToAction(nameof(Index)); // Redirecionar para a página de listagem
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