using Contatos.Enums;

namespace Contatos.Models
{
    public class RegisterAccountModel
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        private PerfilEnum Perfil { get; set; }

    }
}
