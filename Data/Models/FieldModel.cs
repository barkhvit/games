using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.Models

{
    [Table("field")]
    public class FieldModel
    {
        [PrimaryKey][Column("id")] public Guid Id { get; set; }
        [Column("number")] public int Number { get; set; }
        [Column("typeoffield")] public enTypeOfField TypeOfField { get; set; }
    }
}