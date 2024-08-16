using Contatos.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Contatos.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;
        private string sessaoUsuario;
        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public UsuarioModel? BuscarSessao()
        {
            sessaoUsuario = _httpContext.HttpContext.Session.GetString(key: "sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuario)
        {
            string valor = JsonSerializer.Serialize(usuario);

            _httpContext.HttpContext.Session.SetString(key: "sessaoUsuarioLogado", value: valor);
        }

        public void RemoverSessao()
        {
            _httpContext.HttpContext.Session.Remove(key: "sessaoUsuarioLogado");
        }

        public bool ExisteSessao()
        {
            sessaoUsuario = _httpContext.HttpContext.Session.GetString(key: "sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
