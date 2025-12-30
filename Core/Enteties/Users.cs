using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Enteties
{
    public class Users
    {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public string? Username { get; set; }
        public string Alias { get; set; } = "";
        public DateTime LastVisited { get; set; }
    }
}
