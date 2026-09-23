using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Validations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class ConjuntoService : BaseService, IConjuntoService
    {
        private readonly IConjuntoRepository _conjuntoRepository;
        private readonly IAndarRepository _andarRepository;
        private readonly ICondominioRepository _condominioRepository;

        public ConjuntoService(IConjuntoRepository conjuntoRepository,
                              INotificador notificador,
                              IAndarRepository andarRepository,
                              ICondominioRepository condominioRepository) : base(notificador)
        {
            _conjuntoRepository = conjuntoRepository;
            _andarRepository = andarRepository;
            _condominioRepository = condominioRepository;
        }
        public async Task<Conjunto> Adicionar(Conjunto conjunto, int numTorre)
        {
            if (!ExecutarValidacao(new ConjuntoValidation(), conjunto))
                return null;

            foreach (var item in conjunto.Andares)
            {
                foreach (var imovel in item.Imoveis)
                    imovel.Conjunto = numTorre;
            }

            return await _conjuntoRepository.Adicionar(conjunto);
        }
        public async Task Remover(Guid id)
        {
            await _conjuntoRepository.Remover(id);
        }        

        public async Task<Conjunto> AdicionarIguais(Conjunto conjunto, int andares, int imoveis, decimal area, decimal fracao, int numTorre, string NumPrimeiroImovel)
        {
            if (!ExecutarValidacao(new ConjuntoValidation(), conjunto))
                return null;

            var result = await _conjuntoRepository.Adicionar(conjunto);
            if (result != null)
            {
                var numero = TrataNumImovel(NumPrimeiroImovel);
                var letraNumero = NumPrimeiroImovel.ToUpper();
                int numeroImovel = 1;
                int totalAndares = 0;

                while(totalAndares < andares)
                {
                    totalAndares++;
                    List<Imovel> imoveisAnd = new List<Imovel>();
                    int qtdImoveis = 0;

                    if (!numero)
                    {
                        
                    }

                    while (qtdImoveis < imoveis)
                    {
                        GerenciamentoDocImovel doc = new GerenciamentoDocImovel();

                        if (numero)
                        {
                            var imovel = new Imovel
                            {
                                Area = area,
                                NumAndar = totalAndares,
                                Fracao = fracao,
                                NumImovelinterno = numeroImovel,
                                NumImovel = numeroImovel.ToString(),
                                DocImovel = doc
                            };
                            imoveisAnd.Add(imovel);
                        }
                        else
                        {
                            if (letraNumero.Equals("A"))
                                letraNumero = "A";

                            var imovel = new Imovel
                            {
                                Area = area,
                                NumAndar = totalAndares,
                                Fracao = fracao,
                                NumImovelinterno = numeroImovel,
                                NumImovel = totalAndares + "-" + letraNumero,
                                DocImovel = doc
                            };                          
                            imoveisAnd.Add(imovel);
                            letraNumero = RetornaProximaLetra(letraNumero);
                        }
                        qtdImoveis++;
                        numeroImovel++;
                    }

                    Pratico.Dominio.Model.Andar andar = new Pratico.Dominio.Model.Andar
                    {
                        ConjuntoId = result.Id,
                        NumTorre = numTorre,
                        NumAndarInterno = totalAndares,
                        Observacoes = "",
                        Imoveis = imoveisAnd
                    };

                    await _andarRepository.Adicionar(andar);
                    
                }
            }
            return result;
        }

        private bool TrataNumImovel(string numImovel)
        {
            try
            {
                var numero = Convert.ToInt32(numImovel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string RetornaProximaLetra(string letra)
        {
            switch (letra.ToUpper())
            {
                case "A":
                    return "B";
                case "B":
                    return "C";
                case "C":
                    return "D";
                case "D":
                    return "E";
                case "E":
                    return "F";
                case "F":
                    return "G";
                case "G":
                    return "H";
                case "H":
                    return "I";
                case "I":
                    return "J";
                case "J":
                    return "K";
                case "K":
                    return "L";
                case "L":
                    return "M";
                case "M":
                    return "N";
                case "N":
                    return "O";
                case "O":
                    return "P";
                case "P":
                    return "Q";
                case "Q":
                    return "R";
                case "R":
                    return "S";
                case "S":
                    return "T";
                case "T":
                    return "U";
                case "U":
                    return "V";
                case "V":
                    return "W";
                case "W":
                    return "X";
                case "X":
                    return "Y";
                case "Y":
                    return "Z";
                default:
                    return "Z";
            }
        }
        public async Task<IEnumerable<Conjunto>> ObterTodosPorCodigo(int codigo)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
            if (condominio != null)
                return await _conjuntoRepository.Buscar(x => x.CondominioId == condominio.Id);
            return new List<Conjunto>();
            
        }
        public void Dispose()
        {
            _conjuntoRepository?.Dispose();
        }
    }
}
