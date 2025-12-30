using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace Millionaire.Data.Models
{
    [Table("properties")]
    public class PropertiesModel
    {
        [PrimaryKey][Column("id")] public Guid Id { get; set; }
        [Column("gamersid")] public Guid GamersId { get; set; }
        [Column("houses")] public int Houses { get; set; }
        [Column("incollateral")] public bool InCollateral { get; set; } = false;

        [Association(ThisKey = nameof(GamersId), OtherKey = nameof(GamersModel.Id))]
        public GamersModel Gamers { get; set; } = null!;

    }
}
