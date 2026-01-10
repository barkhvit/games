using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Millionaire.Core.DTOs;
using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using Millionaire.GamesManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Millionaire.GamesManager.Manager
{
    public class GameFabric
    {
        // задача класса: получить данные и добавить в базу данных новую игру

        private readonly IUsersService _usersService;
        private readonly IGamesService _gamesService;
        private readonly ILogger<GameFabric> _logger;
        private readonly IMessageService _messageService;

        public GameFabric(IUsersService usersService, IGamesService gamesService, ILogger<GameFabric> logger, IMessageService messageService)
        {
            _usersService = usersService;
            _gamesService = gamesService;
            _logger = logger;
            _messageService = messageService;
        }

        public async Task CreateAsync(string namesOfGames, Guid UserId, CancellationToken ct)
        {
            try
            {
                //получаем пользователя
                var user = await _usersService.GetByIdAsync(UserId, ct);

                //получаем тип игры
                enNamesOfGames _namesOfGames = namesOfGames switch
                {
                    nameof(enNamesOfGames.Monopoly) => enNamesOfGames.Monopoly,
                    nameof(enNamesOfGames.SeaBattle) => enNamesOfGames.SeaBattle,
                    _ => throw new ArgumentException("в GameFabric.CreateAsync неизвестный тип игры")
                };

                if (user != null)
                {
                    //создаем новую игру
                    var game = new Games()
                    {
                        Id = Guid.NewGuid(),
                        User = user,
                        TypeOfGame = _namesOfGames,
                        Name = $"{_namesOfGames.ToString()}_{DateTime.UtcNow.ToString("ddMMyyyy_HHmm")}"
                    };

                    //добавляем в БД
                    var n = await _gamesService.AddAsync(game,ct);

                    //отправляем сообщение пользователю
                    await _messageService.SendMessageAsync(UserId, $"Игра {game.Name} создана.", ct);
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }

    }
}
