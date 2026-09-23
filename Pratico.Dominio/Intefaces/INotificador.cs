using Pratico.Dominio.Notificacoes;
using System.Collections.Generic;

namespace Pratico.Dominio.Intefaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
