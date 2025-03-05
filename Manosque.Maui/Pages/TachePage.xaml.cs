
using ComlineServices;
using Maui.Components;
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class TachePage : ContentPage, IQueryAttributable
    {
        public TachePage()
        {
            InitializeComponent();
        }
        string? TacheId;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("tacheId"))
            {
                TacheId = query["tacheId"] as string;
                ExecuteApi();
            }
        }
        public void ExecuteApi()
        {
            ActionViews.Clear();
            ServiceApi.Command = new ComLineData.ComlineData();
            ServiceApi.Command.Reset();
            ServiceApi.Command.Prompts.Add($@"
                    Connect-Service -Name Data;
                    Get-Action -Tache ""{TacheId}""");
            App.MonServiceApi.Execute();

            var tableList = ServiceApi.Command.Results.TableList;
            if (tableList.Contains("Error"))
            {

            }
            else if (tableList.Contains("Action"))
            {
                ObservableCollection<TacheAction> TacheActions = [];
                var id = default(Guid);
                var list = ServiceApi.Command.Results.Tables["Action"]?.Rows.Cast<DataRow>();
                if (list != null)
                    foreach (var row in list)
                    {
                        var view = new CardViewAction();
                        var site = new TacheAction
                        {
                            Libelle = (string)row["Question"]
                        };

                        view.BindingContext = site;
                        this.ActionViews.Children.Add(view);
                    }
            }
        }

    }

}
