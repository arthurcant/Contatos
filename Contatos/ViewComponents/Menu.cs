using Contatos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Contatos.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult?> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString(key: "sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            UsuarioModel usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);

            return View(usuario);
        }
    }
}
