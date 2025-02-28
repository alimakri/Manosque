using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SqLiteMaui.Models
{
    [SQLite.Table("adresses")]
    public class Adresse
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string Rue { get; set; } = "";

        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string Ville { get; set; } = "";

        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string CodePostal { get; set; } = "";

        public override string ToString()
        {
            return $"{Rue}, {Ville} ({CodePostal})";
        }
    }
}
