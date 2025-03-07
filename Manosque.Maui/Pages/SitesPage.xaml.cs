using ComlineServices;
using Manosque.Maui.Components;
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
        private List<Nav> NavViewModel = [];
        public SitesPage()
        {
            InitializeComponent();
            myDatePicker.PropertyChanged += DatePicker_PropertyChanged;
            NavButtonsRefresh();
        }

        private void NavButtonsRefresh()
        {
            SwipeMenu1.NavButtons.Clear();
            var b = new ButtonWithId { ExecutionId = null, Text = "Home", BackgroundColor = Colors.Transparent, BorderColor = Colors.Transparent };
            b.Clicked += NavButtonClick;
            SwipeMenu1.NavButtons.Children.Add(b);

            foreach (var nav in NavViewModel)
            {
                b = new ButtonWithId { ExecutionId = nav.Id, Text = nav.Libelle, BackgroundColor = Colors.Transparent, BorderColor = Colors.Transparent };
                b.Clicked += NavButtonClick;
                SwipeMenu1.NavButtons.Children.Add(b);
            }
            if (SwipeMenu1.NavButtons.Children.Count > 0) ((ButtonWithId)SwipeMenu1.NavButtons.Children[SwipeMenu1.NavButtons.Children.Count - 1]).TextColor = Colors.Blue;
        }

        private async void NavButtonClick(object? sender, EventArgs e)
        {
            var button = (ButtonWithId)sender;
            var index = NavViewModel.FindIndex(x => x.Id == button.ExecutionId);
            if (index != SwipeMenu1.NavButtons.Children.Count - 1)
            {
                NavViewModel.RemoveRange(index + 1, NavViewModel.Count - index - 1);
                NavButtonsRefresh();
                Execution = button.ExecutionId;
                ExecuteApi();
            }
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
                var list = ServiceApi.Command.Results.Tables["Execution"]?.Rows.Cast<DataRow>();
                if (list != null)
                    foreach (var row in list)
                    {
                        var view = new CardViewSite();
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

                        view.BindingContext = site;
                        this.TaskViews.Children.Add(view);
                    }
            }
        }

        private async void ButtonClick(object obj)
        {
            var site = (Site)obj;
            NavViewModel.Add(new Nav { Id = site.Id, Libelle = site.Libelle });
            NavButtonsRefresh();
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
