namespace Injection
{
    public partial class SecondPage : ContentPage
    {
        public SecondPage(SecondPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
