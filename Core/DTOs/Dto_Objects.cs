using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.DTOs
{
    public static class Dto_Objects
    {
        public static string MainMenuView { get; } = nameof(MainMenuView);
        public static string AboutBotView { get; } = nameof(AboutBotView);
        public static string GamesMainMenuView { get; internal set; } = nameof(GamesMainMenuView);
        public static string CreateGameCommands { get; internal set; } = nameof(CreateGameCommands);

        // REQUESTS
        public static string Request { get; internal set; } = nameof(Request);


        //MONOPOLY
        public static string MonopolyMainMenuView { get; internal set; } = nameof(MonopolyMainMenuView);
        
    }
}
