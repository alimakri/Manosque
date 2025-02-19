using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidMaui
{
    public class Personne
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public Personne() { }

        public Personne(string nom, string prenom)
        {
            Nom = nom;
            Prenom = prenom;
        }

        public override string ToString()
        {
            return $"Nom: {Nom}, Prénom: {Prenom}";
        }
    }
}
