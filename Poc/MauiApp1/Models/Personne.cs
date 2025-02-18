using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Site
    {
        public Guid Id { get; set; }
        public string Libelle { get; set; }
        public string Tache { get; set; }
        public long Statut { get; set; }
    }
}
