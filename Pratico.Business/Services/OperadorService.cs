using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class OperadorService : IOperadorService
    {
        private readonly IOperadorRepository _operadorRepository;
        public OperadorService(IOperadorRepository operadorRepository)
        {
            _operadorRepository = operadorRepository;
        }

        public async Task<Operador> Adicionar(Operador operador)
        {
            return await _operadorRepository.Adicionar(operador);
        }
    }
}
