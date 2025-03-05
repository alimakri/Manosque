using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manosque.Maui.Models
{
    public class Site
    {
        public string Id { get; set; }
        public string? TacheId { get; set; }
        public string Libelle { get; set; } = "";
        public DateOnly Date { get; set; } = default;
        public string? Tache { get; set; } = "";
        public string? Image { get; set; } = "";
        public long Statut { get; set; }
        public Command CardViewCommand { get; set; }
    }
}
