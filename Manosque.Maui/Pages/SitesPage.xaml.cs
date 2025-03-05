using ComlineServices;
using Maui.Components;
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class SitesPage : ContentPage, IQueryAttributable
    {
        //private string? User;
        private string? UserId;
        private string? Execution;
        private Guid? ExecutionId;

        public SitesPage()
        {
            InitializeComponent();
            myDatePicker.PropertyChanged += DatePicker_PropertyChanged;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Init
            UserId = query.TryGetValue("user", out object? value) ? value as string : null;
            Execution = query.TryGetValue("execution", out object? value2) ? value2 as string : "NULL";
            if (query.ContainsKey("date") && DateTime.TryParse((string)query["date"], out DateTime value3)) myDatePicker.Date = value3;
            ExecuteApi();
        }
        private void DatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DatePicker.Date))
            {
                ExecuteApi();
            }
        }
        public void ExecuteApi()
        {
            TaskViews.Clear();

            // Stay here in SitesPage
            ServiceApi.Command = new ComLineData.ComlineData();
            ServiceApi.Command.Reset();
            ServiceApi.Command.Prompts.Add($@"
                    Connect-Service -Name Data;
                    Get-Execution -Execution ""{Execution}"" -Personne ""{UserId}"" -Mode ""Debug"" -DateDebut ""{DateOnly.FromDateTime(myDatePicker.Date)}"" -Filter ""ListeSites""");
            App.MonServiceApi.Execute();

            var tableList = ServiceApi.Command.Results.TableList;
            if (tableList.Contains("Error"))
            {

            }
            else if (tableList.Contains("Execution"))
            {
                ObservableCollection<Site> Sites = [];
                var id = default(Guid);
                var list = ServiceApi.Command.Results.Tables["Execution"]?.Rows.Cast<DataRow>().Where(row => Guid.TryParse(row["Emplacement"].ToString(), out id));
                if (list != null)
                    foreach (var row in list)
                    {
                        var taskView = new CardViewTask();
                        var site = new Site
                        {
                            Id = row["Id"] as string,
                            TacheId = row["tacheId"] as string,
                            Tache = row["Tache"] as string,
                            Libelle = (string)row["Reference"],
                            Statut = (long)row["Statut"],
                            Date = DateOnly.FromDateTime(myDatePicker.Date),
                            Image = "\uf1f1",
                        };
                        site.CardViewCommand = new Command(() => ButtonClick(site));

                        taskView.BindingContext = site;
                        this.TaskViews.Children.Add(taskView);
                    }
            }
        }

        private async void ButtonClick(object obj)
        {
            var site = (Site)obj;
            if (!string.IsNullOrEmpty(site.TacheId))
                await Shell.Current.GoToAsync($"//tache?tacheId={site.TacheId}");
            else
            {
                Execution = site.Id.ToString();
                ExecuteApi();
                //await Shell.Current.GoToAsync($"//sites?execution={site.Id}&user={App.User}&date={site.Date}");
            }

        }

        private async void OnRetour(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");

        }
    }

}
