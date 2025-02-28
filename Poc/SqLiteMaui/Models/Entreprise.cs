using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteMaui.Models
{
    [Table("entreprises")]
    public class Entreprise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nom { get; set; } = "";

        [MaxLength(50)]
        public string Secteur { get; set; } = "";

        public override string ToString()
        {
            return $"{Nom} - {Secteur}";
        }
    }
}
