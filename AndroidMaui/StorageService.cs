using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AndroidMaui
{
    public static class StorageService
    {
        private static readonly Personne DefaultPersonne = new Personne("Nom inconnu", "Prénom inconnu");

        public static void SavePersonne(Personne personne)
        {
            string json = JsonSerializer.Serialize(personne);
            Preferences.Set(nameof(Personne), json); // Utilisation de nameof() pour éviter les erreurs de chaîne
        }

        public static Personne GetPersonne()
        {
            string json = Preferences.Get(nameof(Personne), string.Empty);
            return string.IsNullOrEmpty(json) ? DefaultPersonne : JsonSerializer.Deserialize<Personne>(json);
        }

        public static long GetStorageSize()
        {
            string json = Preferences.Get(nameof(Personne), string.Empty);
            return Encoding.UTF8.GetByteCount(json); // Retourne la taille en octets
        }

        public static string GetMaxStorageSize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "1 Mo par clé (Registry Windows)";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "~1 Mo total (NSUserDefaults macOS)";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Variable (dépend de la configuration Linux)";
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                return "4 Ko par clé (SharedPreferences Android)";
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                return "~1 Mo total (NSUserDefaults iOS)";
            }
            return "Inconnu";
        }
    }
}
