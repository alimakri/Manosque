using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace FileStorageMaui.Services
{
    public static class FileService
    {
        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "userdata.json");

        public static async Task SaveUserDataAsync(string nom, string prenom)
        {
            List<Personne> personnes = await LoadAllUserDataAsync(); // Charge la liste existante
            personnes.Add(new Personne(nom, prenom)); // Ajoute une nouvelle personne

            string json = JsonSerializer.Serialize(personnes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        public static async Task<List<Personne>> LoadAllUserDataAsync()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Personne>(); // Retourne une liste vide si le fichier n'existe pas
            }

            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Personne>>(json) ?? new List<Personne>();
        }

        public static void DeleteUserData()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }

    public class Personne
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public Personne(string nom, string prenom)
        {
            Nom = nom;
            Prenom = prenom;
        }

        public override string ToString()
        {
            return $"{Nom} {Prenom}";
        }
    }
}
