using Contatos.Filters;
using Contatos.Helper;
using Contatos.Models;
using Contatos.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers
{
    [RestritoAdmin]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IContatoRepositorio _contatoRepositorio;
        public UsuarioController(IUsuarioRepositorio UsuarioRepositorio, IContatoRepositorio contatoRepositorio)
        {
            this._usuarioRepositorio = UsuarioRepositorio;
            this._contatoRepositorio = contatoRepositorio;
        }
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario = _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar o usuário. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenhaModel)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil
                    };

                    usuario = _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário atualizado com sucesso.";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o usuário. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoverConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id: id);
            return View(usuario);
        }

        public IActionResult Remover(int id)
        {
            try
            {
                bool removido = _usuarioRepositorio.Remover(id);
                if (removido)
                {
                    TempData["MensagemSucesso"] = "Usuário removido com sucesso.";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível remover esse usuário.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos remover o usuário. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ListarContatosPorUsuarioId(int id)
        {
            try
            {
                List<ContatoModel> contatos = _contatoRepositorio.BuscarContatos(id);
                return PartialView(viewName: "_ContatosUsuario", model: contatos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        
      
    }
}
