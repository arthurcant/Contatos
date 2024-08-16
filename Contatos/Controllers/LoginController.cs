using Contatos.Helper;
using Contatos.Models;
using Contatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;
        private readonly IConfiguration _config;
        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email, IConfiguration config)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
            _config = config;
        }
        public IActionResult Index()
        {
            if (_sessao.BuscarSessao() != null) return RedirectToAction(actionName: "Index", controllerName: "Home");
            return View();
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }
        public IActionResult Sair()
        {
            _sessao.RemoverSessao();
            return RedirectToAction(actionName: "Index", controllerName: "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarUsuario(loginModel.Login);

                    if (usuario != null)
                    {
                        if (!usuario.SenhaValida(senha: loginModel.Senha))
                        {
                            usuario.SetHashSenha();
                            _usuarioRepositorio.Atualizar(usuario);
                        }

                        if (usuario.SenhaValida(senha: loginModel.Senha))
                        {
                            _sessao.CriarSessao(usuario);
                            return RedirectToAction(actionName: "Index", controllerName: "Home");
                        }
                        TempData["MensagemErro"] = $"Senha inválida. Por favor, tente novamente.";
                    }
                    TempData["MensagemErro"] = $"Usuário e/ou senha inválidos.";
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult EnviarLinkEmail(RedefinirSenhaModel redefinirSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarEmailELogin(email: redefinirSenha.Email, login: redefinirSenha.Login);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();

                        string mensagem = $"Sua nova senha é: {novaSenha}";

                        Email email = new Email(_config.GetValue<string>(key: "SMTP:Host"), _config.GetValue<string>(key: "SMTP:UserName"), _config.GetValue<string>(key: "SMTP:Senha"));
                        bool emailEnviado =  email.SendEmail(new List<string> { $"{redefinirSenha.Email}" },subject: "Sistema de Contatos - Nova Senha", body: mensagem);

                        if(emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = $"Enviamos para seu email uma nova senha.";
                           return RedirectToAction(actionName: "Index", controllerName: "Login");
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não foi possível enviar o email com sua nova senha. Por favor, tente novamente.";
                        }
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Não conseguimos redefinir sua senha. Por favor, verifique os dados informados.";
                    }
                }
                return View(nameof(RedefinirSenha));
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha. Tente novamente. Detalhe: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RegisterAnAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAnAccount(UsuarioModel usuario)
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

    }
}
