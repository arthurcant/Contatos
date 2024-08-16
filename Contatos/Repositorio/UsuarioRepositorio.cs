using Contatos.Data;
using Contatos.Helper;
using Contatos.Models;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _bancoContext;
        private readonly IHttpContextAccessor _httpContext;
        public UsuarioRepositorio(BancoContext bancoContext, IHttpContextAccessor httpContext)
        {
            this._bancoContext = bancoContext;
            this._httpContext = httpContext;
        }
        public UsuarioModel ListarPorId(int id)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
        }
        public UsuarioModel BuscarEmailELogin(string email, string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }
        public UsuarioModel BuscarUsuario(string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }
        public List<UsuarioModel> BuscarTodos()
        {
            return _bancoContext.Usuarios.Include(x => x.Contatos).ToList();
        }
        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetHashSenha();
            _bancoContext.Usuarios.Add(usuario);
            _bancoContext.SaveChanges();
            return usuario;
        }
        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDB = ListarPorId(usuario.Id);
            if (usuarioDB == null) throw new Exception("Houve um erro na atualização do usuário.");
            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Perfil = usuario.Perfil;
            usuarioDB.Email = usuario.Email;
            usuarioDB.DataAlteracao = DateTime.Now;

            _bancoContext.Usuarios.Update(usuarioDB);
            _bancoContext.SaveChanges();
            return usuarioDB;
        }
        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDB = ListarPorId(id: alterarSenhaModel.Id);
            if (usuarioDB is null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            else
            {
                if (!usuarioDB.SenhaValida(senha: alterarSenhaModel.SenhaAtual))
                {
                    throw new Exception("Senha atual não confere.");
                }
                else if (usuarioDB.SenhaValida(senha: alterarSenhaModel.NovaSenha))
                {
                    throw new Exception("Nova senha deve ser diferente da senha atual.");
                }
                else
                {
                    usuarioDB.SetNovaSenha(alterarSenhaModel.NovaSenha);
                    usuarioDB.DataAlteracao = DateTime.Now;

                    _bancoContext.Usuarios.Update(usuarioDB);
                    _bancoContext.SaveChanges();
                    return usuarioDB;
                }
            }
        }
        public bool Remover(int id)
        {
            Sessao sessao = new Sessao(_httpContext);

            UsuarioModel usuarioDB = ListarPorId(id);
            if (usuarioDB is null) throw new Exception("Houve um erro na exclusão do contato.");
            else
            {
                if (sessao.ExisteSessao())
                {
                    _bancoContext.Usuarios.Remove(usuarioDB);
                    _bancoContext.SaveChanges();
                    return true;

                }
                else
                {
                    throw new Exception("Acesso não autorizado");
                }
            }
        }
    }
}
