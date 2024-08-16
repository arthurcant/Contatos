using Contatos.Enums;
using Contatos.Helper;
using System.ComponentModel.DataAnnotations;

namespace Contatos.Models
{
    public class UsuarioModel
    {
        private int _id;
        private string _nome;
        private string _login;
        private string _senha;
        private string _email;
        private PerfilEnum? _perfil;
        private DateTime _dataCadastro;
        private DateTime? _dataAlteracao;

        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Por favor, digite o nome do usuário")]
        public string Nome { get => _nome; set => _nome = value; }

        [Required(ErrorMessage = "Por favor, digite o login do usuário")]
        public string Login { get => _login; set => _login = value; }

        [Required(ErrorMessage = "Por favor, digite a senha do usuário")]
        public string Senha { get => _senha; set => _senha = value; }

        [Required(ErrorMessage = "Por favor, digite o email do usuário")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string Email { get => _email; set => _email = value; }

        [Required(ErrorMessage = "Informe o perfil do usuário")]
        public PerfilEnum? Perfil { get => _perfil; set => _perfil = value; }
        public DateTime DataCadastro { get => _dataCadastro; set => _dataCadastro = value; }
        public DateTime? DataAlteracao { get => _dataAlteracao; set => _dataAlteracao = value; }
        public bool SenhaValida(string senha)
        {
            return Senha == senha.Criptografar();
        }
        public void SetHashSenha()
        {
            Senha = Senha.Criptografar();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.Criptografar();
            return novaSenha;
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.Criptografar();
        }
        public virtual List<ContatoModel>? Contatos { get; set; }
    }
}
