using Millionaire.Core.Enteties;
using Millionaire.Data.Models;
using Millionaire.GamesManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.DataContext
{
    internal static class ModelMapper
    {
        public static Games MapFromModel(GamesModel model)
        {
            return new Games()
            {
                Id = model.Id,
                User = MapFromModel(model.User),
                Name = model.Name,
                TypeOfGame = Enum.Parse<enNamesOfGames>(model.TypeOfGame),
                FinishScore = model.FinishScore,
                CreateAt = model.CreateAt,
                IsActive = model.IsActive,
                CountOfRound = model.CountOfRound
            };
        }

        public static GamesModel MapToModel(Games entity)
        {
            return new GamesModel()
            {
                Id = entity.Id,
                AdminUserId = entity.User.Id,
                Name = entity.Name,
                TypeOfGame = entity.TypeOfGame.ToString(),
                FinishScore = entity.FinishScore,
                CreateAt = entity.CreateAt,
                IsActive = entity.IsActive,
                CountOfRound = entity.CountOfRound
            };
        }

        public static Users MapFromModel(UserModel model)
        {
            return new Users()
            {
                Id = model.Id,
                TelegramId = model.TelegramId,
                Username = model.Username,
                Alias = model.Alias,
                LastVisited = model.LastVisited
            };
        }

        public static UserModel MapToModel(Users entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                TelegramId = entity.TelegramId,
                Username = entity.Username,
                Alias = entity.Alias,
                LastVisited = entity.LastVisited
            };
        }
    }
}
