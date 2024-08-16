using Contatos.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers
{
    [UsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
