using System.Threading;
using System.Threading.Tasks;
using Pratico.Dominio.Model;

namespace Pratico.Worker.OutBox
{
    public interface IOutBoxMessageQueue
    {
        Task<OutBoxMessage> EnfileirarAsync(string tipo, string conteudo, CancellationToken cancellationToken = default);
    }
}
