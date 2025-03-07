using ComlineServices;
using Manosque.Maui.Components;
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class TachePage : ContentPage, IQueryAttributable
    {
        private string? TacheId;
        public TachePage()
        {
            InitializeComponent();
        }

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
                    Get-Action -Tache ""{TacheId}"" -Mode Debug");
            App.MonServiceApi.Execute();

            var tableList = ServiceApi.Command.Results.TableList;
            if (tableList.Contains("Error"))
            {

            }
            else if (tableList.Contains("Action"))
            {
                ObservableCollection<CardViewModel> TacheActions = [];
                var id = default(Guid);
                var list = ServiceApi.Command.Results.Tables["Action"]?.Rows.Cast<DataRow>();
                if (list != null)
                    foreach (var row in list)
                    {
                        var type = (long)row["Type"];
                        switch (type)
                        {
                            case 2:
                                // View
                                var view2 = new CardView1();
                                this.ActionViews.Children.Add(view2);
                                // ViewModel
                                var viewModel2 = new CardViewModel();
                                viewModel2.Add("Question1", new CardViewModelItem { Libelle = (string)row["Question"], TypeAction = "CheckBox", Valeur = "False" });
                                viewModel2.Add("Question2", new CardViewModelItem { Libelle = "bla bla", TypeAction = "CheckBox", Valeur = "True" });
                                viewModel2.Add("Question3", new CardViewModelItem { Libelle = "Alors ?", TypeAction = "TextBox", Valeur = "Bonjour Ali" });
                                // View|ViewModel
                                view2.BindingContext = new
                                {
                                    Items = viewModel2,
                                    CardTheme = "LightBlue",
                                    Statut = Application.Current.Resources["BackgroundCard1"] as LinearGradientBrush,
                                    Validation = 1
                                };

                                break;
                            default:
                                // View
                                var viewDefault = new CardViewAction();
                                this.ActionViews.Children.Add(viewDefault);
                                // ViewModel
                                var viewModelDefault = new CardViewModel();
                                viewModelDefault.Add("Question1", new CardViewModelItem { Libelle = (string)row["Question"], TypeAction = "CheckBox", Valeur = "True" });
                                // View|ViewModel
                                viewDefault.BindingContext = new
                                {
                                    Items = viewModelDefault,
                                    CardTheme = "LightYellow",
                                    Statut = Application.Current.Resources["BackgroundCard2"] as LinearGradientBrush,
                                    Validation = 0.5
                                };
                                //     public LinearGradientBrush StatutGradientBrush => (LinearGradientBrush)Application.Current.Resources["StatutGradientBrush"];
                                break;
                        }

                    }
            }
        }

    }

}
