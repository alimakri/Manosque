using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manosque.Maui.Models
{
    public class CardViewModel : Dictionary<string, CardViewModelItem>
    {
        public Guid Id { get; set; }
        public string Libelle { get; set; } = "";
    }
    public class CardViewModelItem
    {
        public string Libelle { get; set; }
        public string Valeur { get; set; }
        public string TypeAction { get; set; }

    }
}
