using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Comtele.Sdk.Core.Resources;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface ISmsService
    {
        Task<ServiceResult<object>> SendAsync(string content, params string[] receivers);
    }
}
