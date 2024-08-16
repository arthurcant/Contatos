using Contatos.Models;

namespace Contatos.Repositorio
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel BuscarUsuario(string login);
        UsuarioModel BuscarEmailELogin(string email, string login);
        UsuarioModel ListarPorId(int id);
        List<UsuarioModel> BuscarTodos();
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Atualizar(UsuarioModel usuario);
        UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel);
        bool Remover(int id);
    }
}
