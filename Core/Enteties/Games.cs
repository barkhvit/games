
using Millionaire.GamesManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Enteties
{
    public class Games
    {
        public Guid Id { get; set; }
        public Users User { get; set; } = null!;
        public enNamesOfGames TypeOfGame { get; set; }
        public string Name { get; set; } = String.Empty;
        public int FinishScore { get; set; } = 0;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public int CountOfRound { get; set; } = 1;
    }
}
