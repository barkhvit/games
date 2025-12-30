using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.Models
{
    [Table("gamers")]
    public class GamersModel 
    {
        [PrimaryKey][Column("id")]public Guid Id { get; set; }
        [Column("gameid")] public Guid GameId { get; set; }
        [Column("userid")] public Guid UserId { get; set; }
        [Column("fieldid")] public Guid FieldId { get; set; }
        [Column("gamerslevel")] public enGamersLevel Gamerslevel { get; set; } = enGamersLevel.junior;
        [Column("alias")] public string Alias { get; set; } = "";
        [Column("score")] public int Score { get; set; } = 200;
        [Column("order")] public int Order { get; set; } // Порядок хода
        [Column("isactive")] public bool IsActive { get; set; } = true;

        [Association(ThisKey = nameof(GameId), OtherKey = nameof(GamesModel.Id))]
        public GamesModel Games { get; set; } = null!;

        [Association(ThisKey = nameof(UserId), OtherKey = nameof(UserModel.Id))]
        public UserModel User { get; set; } = null!;

        [Association(ThisKey = nameof(FieldId), OtherKey = nameof(FieldModel.Id))]
        public FieldModel Field { get; set; } = null!;
    }
}
