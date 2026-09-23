using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IOperadorService
    {
        Task<Operador> Adicionar(Operador operador);
    }
}
