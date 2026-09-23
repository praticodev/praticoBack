using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IUsuarioCondominioService
    {
        Task<UsuarioCondominio> Adicionar(UsuarioCondominio morador);
        Task<bool> AdicionarUsuarioImovel(Pessoa morador, PessoaUsuario usuario);
    }
}
