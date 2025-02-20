using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SqLiteMaui.Models;

namespace SqLiteMaui.Services
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _Database;

        public Database()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
            _Database = new SQLiteAsyncConnection(dbPath);

            _Database.CreateTableAsync<Personne>().Wait();
            _Database.CreateTableAsync<Adresse>().Wait();
            _Database.CreateTableAsync<Entreprise>().Wait();
        }

        public Task<int> SavePersonneAsync(Personne personne)
        {
            return _Database.InsertAsync(personne);
        }

        public Task<List<Personne>> GetPersonnesAsync()
        {
            return _Database.Table<Personne>().ToListAsync();
        }

        public Task<int> SaveAdresseAsync(Adresse adresse)
        {
            return _Database.InsertAsync(adresse);
        }

        public Task<int> SaveEntrepriseAsync(Entreprise entreprise)
        {
            return _Database.InsertAsync(entreprise);
        }

        public Task<int> ClearDatabase()
        {
            return _Database.DeleteAllAsync<Personne>();
        }
    }
}
