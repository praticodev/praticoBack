using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IRepresentanteLegalService : IDisposable
    {
        Task<RepresentanteLegal> Adicionar(RepresentanteLegal representante);
        Task<bool> Atualizar(RepresentanteLegal representante);
    }
}
