using Millionaire.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Interfaces
{
    public interface IMessageService
    {
        Task SendMessage(Guid Id, string Text, CancellationToken ct);
    }
}
