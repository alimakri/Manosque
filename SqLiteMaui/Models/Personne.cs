using System.ComponentModel.DataAnnotations;
using SQLite;

namespace SqLiteMaui.Models
{
    [Table("personnes")]
    public class Personne
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string Nom { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string Prenom { get; set; }

        public int AdresseId { get; set; } // Clé étrangère vers Adresse
        public int EntrepriseId { get; set; } // Clé étrangère vers Entreprise

        // 🚀 Ajout des objets liés (non stockés en base)
        [Ignore]
        public Adresse Adresse { get; set; }

        [Ignore]
        public Entreprise Entreprise { get; set; }

        public Personne() { }

        public Personne(string nom, string prenom, int adresseId, int entrepriseId)
        {
            Nom = nom;
            Prenom = prenom;
            AdresseId = adresseId;
            EntrepriseId = entrepriseId;
        }

        public override string ToString()
        {
            string adresseInfo = Adresse != null ? $"📍 {Adresse.Rue}, {Adresse.Ville} ({Adresse.CodePostal})" : "📍 Adresse inconnue";
            string entrepriseInfo = Entreprise != null ? $"🏢 {Entreprise.Nom} - {Entreprise.Secteur}" : "🏢 Entreprise inconnue";

            return $"[{Id}] {Nom} {Prenom}\n{adresseInfo}\n{entrepriseInfo}";
        }
    }
}
