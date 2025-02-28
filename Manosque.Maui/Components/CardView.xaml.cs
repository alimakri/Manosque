using Manosque.Maui;

namespace MauiApp1.Components;

public partial class CardView : Frame
{
    #region Date
    public static readonly BindableProperty DateProperty = BindableProperty.Create(
        nameof(Date),
        typeof(string),
        typeof(CardView),
        null,
        propertyChanged: null);

    public string Date
    {
        get => (string)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
    #endregion

    #region Execution
    public static readonly BindableProperty ExecutionProperty = BindableProperty.Create(
        nameof(Execution),
        typeof(string),
        typeof(CardView),
        null,
        propertyChanged: OnExecutionChanged);

    public string Execution
    {
        get => (string)GetValue(ExecutionProperty);
        set => SetValue(ExecutionProperty, value);
    }
    private static void OnExecutionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CardView cardView) return;

        cardView.ExecutionLabel.Text = (string)newValue;
    }
    #endregion


    public static readonly BindableProperty TacheProperty
        = BindableProperty.Create(nameof(Tache),
            typeof(string), typeof(CardView), "-",
            propertyChanged: OnTacheChanged);

    public string Tache
    {
        get => (string)GetValue(TacheProperty);
        set => SetValue(TacheProperty, value);
    }

    public static readonly BindableProperty StatutProperty
    = BindableProperty.Create(nameof(Statut),
        typeof(string), typeof(CardView), "-",
        propertyChanged: OnStatutChanged);

    public string Statut
    {
        get => (string)GetValue(StatutProperty);
        set => SetValue(StatutProperty, value);
    }

    public string CardImage
    {
        get { return cardImage; }
        set
        {
            cardImage = value;
            CreditCardImageLabel.Text = value;
            CreditCardImageLabel.FontFamily = "FA6Brands";
        }
    }
    private string cardImage = "";


    public static readonly BindableProperty CardThemeProperty
    = BindableProperty.Create(nameof(CardTheme),
        typeof(string), typeof(CardView), "-",
        propertyChanged: OnCardThemeChanged);

    public string CardTheme
    {
        get => (string)GetValue(CardThemeProperty);
        set => SetValue(CardThemeProperty, value);
    }
    private static void OnCardThemeChanged(BindableObject bindable,
    object oldValue, object newValue)
    {
        if (bindable is not CardView creditCardView)
            return;

        creditCardView.SetCardTheme();
    }
    private void SetCardTheme()
    {
        if (string.IsNullOrEmpty(Tache)) CreditCardViewFrame.BackgroundColor = Colors.White; else CreditCardViewFrame.BackgroundColor = Colors.SkyBlue;
    }



    private bool bandeauEnabled;

    public bool BandeauEnabled
    {
        get { return bandeauEnabled; }
        set { bandeauEnabled = value; Bandeau.Opacity = value ? 1 : 0.5; }
    }

    private static void OnTacheChanged(BindableObject bindable,
        object oldValue, object newValue)
    {
        if (bindable is not CardView creditCardView)
            return;

        creditCardView.SetTache();
    }

    private static void OnStatutChanged(BindableObject bindable,
        object oldValue, object newValue)
    {
        if (bindable is not CardView creditCardView)
            return;

        creditCardView.SetStatut();
    }


    public CardView()
    {
        InitializeComponent();


        CreditCardImageLabel.Text = "\uf09d";
        CreditCardImageLabel.FontFamily = "FA6Regular";

        TacheLabel.Text = "-";

        StatutLabel.Text = "-";
    }

    private void SetTache()
    {
        TacheLabel.Text = Tache;
        CardTheme = "Pink";
    }

    private void SetStatut()
    {
        StatutLabel.Text = Statut;
    }

    private async void OnTacheClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Tache))
            await Shell.Current.GoToAsync($"//tache?execution={Execution}&user={App.User}&tache={Tache}");
        else
            await Shell.Current.GoToAsync($"//sites?execution={Execution}&user={App.User}&tache={Tache}&date={Date}");
    }
}