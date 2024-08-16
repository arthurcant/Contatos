using System.ComponentModel.DataAnnotations;

namespace Contatos.Models
{
    public class ContatoModel
    {
        private int _id;
        private string _nome;
        private string _email;
        private string _telefone;
        
        public int Id { get => _id; set => _id = value; }
        [Required(ErrorMessage = "Por favor, digite o nome do contato")]
        public string Nome { get => _nome; set => _nome = value; }

        [Required(ErrorMessage = "Por favor, digite o email do contato")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string Email { get => _email; set => _email = value; }

        [Required(ErrorMessage = "Por favor, digite o telefone do contato")]
        [Phone(ErrorMessage = "O telefone informado não é válido")]
        public string Telefone { get => _telefone; set => _telefone = value; }

        public int? UsuarioId { get; set; }

        public UsuarioModel? Usuario { get; set; }
    }
}
