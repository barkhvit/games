using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using Millionaire.GamesManager.Enums;

namespace Millionaire.Data.Models
{
    [Table("games")]
    public class GamesModel
    {
        [PrimaryKey][Column("id")] public Guid Id { get; set; }
        [Column("adminuserid")] public Guid AdminUserId { get; set; }
        [Column("name")] public string Name { get; set; } = String.Empty;
        [Column("typeofgame")] public string TypeOfGame { get; set; } = String.Empty;
        [Column("finishscore")] public int FinishScore { get; set; } = 0;
        [Column("createat")] public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        [Column("isactive")] public bool IsActive { get; set; } = true;


        [Association(ThisKey = nameof(AdminUserId), OtherKey = nameof(UserModel.Id))]
        public UserModel User { get; set; } = null!;
    }

    
}
