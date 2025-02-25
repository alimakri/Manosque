
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class ComlinePage : ContentPage
    {
        public ComlinePage()
        {
            InitializeComponent();
        }

        private async void OnRetour(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//root/login");

        }

        private void OnComline(object sender, EventArgs e)
        {
            var command = App.MonServiceAPi.Command;
            command.Prompt = Prompt.Text;
            App.MonServiceAPi.Execute([command.Prompt]);
            DataTablesStackLayout.Clear();
            try
            {
                foreach (DataTable dataTable in command.Results.Tables)
                {

                    CreateGrid(dataTable);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion : {ex.Message}");
            }
        }
        private void CreateGrid(DataTable MyDataTable)
        {
            DataTablesStackLayout.Children.Add(new Label
            {
                Text = MyDataTable.TableName,
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                TextColor = Colors.Navy
            });

            var grid = new Grid
            {
                ColumnSpacing = 1,
                RowSpacing = 1
            };

            // Ajouter les colonnes et les lignes du DataTable au Grid
            for (int col = 0; col < MyDataTable.Columns.Count; col++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int row = 0; row <= MyDataTable.Rows.Count; row++) // Une ligne supplémentaire pour les en-têtes
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Ajouter les en-têtes des colonnes
            for (int col = 0; col < MyDataTable.Columns.Count; col++)
            {
                var headerFrame = new Frame
                {
                    BorderColor = Colors.Black,
                    BackgroundColor = Colors.Silver,
                    Padding = new Thickness(5),
                    Content = new Label
                    {
                        Text = MyDataTable.Columns[col].ColumnName,
                        FontAttributes = FontAttributes.Bold
                    }
                };
                Grid.SetColumn(headerFrame, col);
                Grid.SetRow(headerFrame, 0);
                grid.Children.Add(headerFrame);
            }

            // Ajouter les données des lignes
            for (int row = 0; row < MyDataTable.Rows.Count; row++)
            {
                for (int col = 0; col < MyDataTable.Columns.Count; col++)
                {
                    var cellFrame = new Frame
                    {
                        BorderColor = Colors.Black,
                        Padding = new Thickness(5),
                        Content = new Label
                        {
                            Text = MyDataTable.Rows[row][col].ToString()
                        }
                    };
                    Grid.SetColumn(cellFrame, col);
                    Grid.SetRow(cellFrame, row + 1); // row + 1 car la première ligne est pour les en-têtes
                    grid.Children.Add(cellFrame);
                }
            }

            // Ajouter le Grid au StackLayout principal
            DataTablesStackLayout.Children.Add(grid);
        }
    }

}
