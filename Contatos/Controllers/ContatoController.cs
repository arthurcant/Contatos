using Contatos.Filters;
using Contatos.Helper;
using Contatos.Models;
using Contatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers
{
    [UsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ISessao _sessao;
        public ContatoController(IContatoRepositorio ContatoRepositorio, ISessao sessao)
        {
            this._contatoRepositorio = ContatoRepositorio;
            this._sessao = sessao;
        }
        public IActionResult Index()
        {
            UsuarioModel usuarioLogado = _sessao.BuscarSessao();
            List<ContatoModel> contatos = _contatoRepositorio.BuscarContatos(usuarioLogado.Id);
            return View(contatos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessao();
                    contato.UsuarioId = usuarioLogado.Id;

                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {

                    return View(contato);
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar o contato. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id: id);
            return View(contato);
        }

        public IActionResult RemoverConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id: id);
            return View(contato);
        }
        public IActionResult Remover(int id)
        {
            try
            {
                bool removido = _contatoRepositorio.Remover(id);
                if (removido)
                {
                    TempData["MensagemSucesso"] = "Contato removido com sucesso.";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível remover esse contato.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos remover o contato. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
      
        [HttpPost]
        public IActionResult Editar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessao();
                    contato.UsuarioId = usuarioLogado.Id;

                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato atualizado com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(contato);
                }
            }
            catch (Exception ex)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o contato. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
