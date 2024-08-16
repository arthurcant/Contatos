using Contatos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contatos.Models
{
    public class UsuarioSemSenhaModel
    {
        private int _id;
        private string _nome;
        private string _login;
        private string _email;
        private PerfilEnum? _perfil;
        

        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Por favor, digite o nome do usuário")]
        public string Nome { get => _nome; set => _nome = value; }

        [Required(ErrorMessage = "Por favor, digite o login do usuário")]
        public string Login { get => _login; set => _login = value; }

        [Required(ErrorMessage = "Por favor, digite o email do usuário")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string Email { get => _email; set => _email = value; }

        [Required(ErrorMessage = "Informe o perfil do usuário")]
        public PerfilEnum? Perfil { get => _perfil; set => _perfil = value; }
    }
}
