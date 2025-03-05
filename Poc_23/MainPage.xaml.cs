using Poc23.CardViews;

namespace Poc_23
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


        }

        private void Go_Clicked(object sender, EventArgs e)
        {
            var cardView1 = new CardViewText();
            var cardView2 = new CardViewChoice();
            MaLayout.Children.Add(cardView1);
            MaLayout.Children.Add(cardView2);

        }
    }

}
