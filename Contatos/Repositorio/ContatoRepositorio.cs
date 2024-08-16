using Contatos.Data;
using Contatos.Filters;
using Contatos.Helper;
using Contatos.Models;

namespace Contatos.Repositorio
{
    
    public class ContatoRepositorio : IContatoRepositorio
    {
        private readonly BancoContext _bancoContext;
        private readonly IHttpContextAccessor _httpContext;
        public ContatoRepositorio(BancoContext bancoContext, IHttpContextAccessor httpContext)
        {
            this._bancoContext = bancoContext;
            this._httpContext = httpContext; 
        }
        public List<ContatoModel> BuscarContatos(int usuarioId)
        {
            return _bancoContext.Contatos.Where(x => x.UsuarioId == usuarioId).ToList();
        }
        public ContatoModel Adicionar(ContatoModel contato)
        {
            _bancoContext.Contatos.Add(contato);
            _bancoContext.SaveChanges();
            return contato;
        }

        public ContatoModel ListarPorId(int id)
        {
            return _bancoContext.Contatos.FirstOrDefault(x => x.Id == id);
        }

        public ContatoModel Atualizar(ContatoModel contato)
        {
            ContatoModel contatoDB = ListarPorId(contato.Id);
            if (contatoDB == null) throw new Exception("Houve um erro na atualização do contato.");
            contatoDB.Nome = contato.Nome;
            contatoDB.Email = contato.Email;
            contatoDB.Telefone = contato.Telefone;

            _bancoContext.Contatos.Update(contatoDB);
            _bancoContext.SaveChanges();
            return contatoDB;
        }

        //[UsuarioLogado]
        public bool Remover(int id)
        {
            Sessao sessao = new Sessao(_httpContext);
            
            ContatoModel contatoDB = ListarPorId(id);
            if (contatoDB is null) throw new Exception("Houve um erro na exclusão do contato.");
            else
            {
                if (sessao.ExisteSessao())
                {
                    _bancoContext.Contatos.Remove(contatoDB);
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
