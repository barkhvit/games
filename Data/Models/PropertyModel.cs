using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace Millionaire.Data.Models
{
    [Table("property")]
    public class PropertyModel
    {
        [PrimaryKey][Column("id")] public Guid Id { get; set; }
        [Column("number")] public int Number { get; set; }
        [Column("name")] public string Name { get; set; } = "";
        [Column("color")] public enFieldColor FieldColor {get; set;}
        [Column("price")] public int Price { get; set; }
        [Column("rent")] public int Rent { get; set; }
    }

    
}
