using ComlineApp.Manager;
using ComlineApp.Services;
using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class MainPageViewModel
    {
        IAlertService AlertService;
        private readonly IServiceApi MonServiceApi;
        public ObservableCollection<Site> Sites { get; set; }
        private void ShowAlert(string message)
        {
            AlertService.ShowAlert("Alerte", message, "OK");
        }
        public MainPageViewModel(IServiceApi serviceApi)
        {
            MonServiceApi = serviceApi;

            Sites = new ObservableCollection<Site>();
            //AlertService = alertService;
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            MonServiceApi.Command.Prompt = $@"Get-Execution -Personne ""mohamed"" -DateDebut ""{today}"" -Filter ""ListeSites""";
            MonServiceApi.Command.Results.Tables.Clear();
            MonServiceApi.Execute();
            DataTable tableSites;
            try
            {
                tableSites = MonServiceApi.Command.Results.Tables[0];
            }
            catch (Exception ex)
            {
                return;
            }
            if (tableSites.TableName == "Error")
            {
                //ShowAlert((string)tableSites.Rows[0]["Libelle"]);

            }
            else
            {
                var id = default(Guid);
                foreach (DataRow row in tableSites.Rows)
                {
                    if (Guid.TryParse(row["Emplacement"].ToString(), out id))
                        Sites.Add(new Site { Id = id, Tache = (string)row["Tache"], Libelle = (string)row["Reference"], Statut = (long)row["Statut"] });
                }
            }
        }
    }
}
