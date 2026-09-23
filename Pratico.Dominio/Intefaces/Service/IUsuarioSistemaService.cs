using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IUsuarioSistemaService
    {
        Task<UsuarioSistema> Adicionar(UsuarioSistema usuario);
        Task<UsuarioSistema> Buscar(string usuarioId, Guid CondominioId);
    }
}
