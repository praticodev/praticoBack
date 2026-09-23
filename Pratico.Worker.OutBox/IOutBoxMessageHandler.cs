using System.Threading;
using System.Threading.Tasks;
using Pratico.Dominio.Model;

namespace Pratico.Worker.OutBox
{
    public interface IOutBoxMessageHandler
    {
        string Tipo { get; }
        Task ProcessarAsync(OutBoxMessage mensagem, CancellationToken cancellationToken);
    }
}
