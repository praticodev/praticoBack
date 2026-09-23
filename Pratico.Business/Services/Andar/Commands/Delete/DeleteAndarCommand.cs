using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Business.Services.Andar.Commands.Delete
{
    public class DeleteAndarCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
