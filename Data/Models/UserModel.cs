using System;
using System.Collections.Generic;
using LinqToDB.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.Models
{
    [Table("user")]
    public class UserModel
    {
        [PrimaryKey][Column("id")]public Guid Id { get; set; }
        [Column("telegramid")] public long TelegramId { get; set; }
        [Column("username")] public string? Username { get; set; }
        [Column("alias")] public string Alias { get; set; } = "";
        [Column("lastvisited")] public DateTime LastVisited { get; set; }
    }
}
