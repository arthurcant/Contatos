using Contatos.Helper;
using Contatos.Models;
using Contatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers
{
    public class AlterarSenhaController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public AlterarSenhaController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            this._usuarioRepositorio = usuarioRepositorio;
            this._sessao = sessao;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Alterar(AlterarSenhaModel alterarSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessao();
                    alterarSenha.Id = usuarioLogado.Id;
                    _usuarioRepositorio.AlterarSenha(alterarSenhaModel: alterarSenha);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso.";
                    return View(viewName: "Index", model: alterarSenha);
                }
                return View(viewName: "Index", model: alterarSenha);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível alterar a senha. Detalhe: {ex.Message}";
                return View(viewName: "Index", model: alterarSenha);
            }
        }
    }
}
