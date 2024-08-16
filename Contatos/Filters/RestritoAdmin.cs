using Contatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Contatos.Filters
{
    public class RestritoAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Login" },
                    {"action", "Index"}
                });
            }
            else
            {
                UsuarioModel usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);
                if (usuario is null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Login" },
                    {"action", "Index"}
                    });
                }
                if(usuario.Perfil != Enums.PerfilEnum.Administrador)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Restrito" },
                    {"action", "Index"}
                });
                }
            }

            base.OnActionExecuted(context);
        }
    }
}
