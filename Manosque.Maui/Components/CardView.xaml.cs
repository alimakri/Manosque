namespace MauiApp1.Components;

public partial class CardView : Frame
{
    #region CardNumber
    public static readonly BindableProperty CardNumberProperty = BindableProperty.Create(
        nameof(CardNumber),
        typeof(string),
        typeof(CardView),
        null,
        propertyChanged: OnCardNumberChanged);

    public string CardNumber
    {
        get => (string)GetValue(CardNumberProperty);
        set => SetValue(CardNumberProperty, value);
    }
    private static void OnCardNumberChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CardView cardView) return;

        cardView.CardNumberLabel.Text = (string)newValue;
    }
    #endregion



    public static readonly BindableProperty ExpirationDateProperty
        = BindableProperty.Create(nameof(ExpirationDate),
            typeof(string), typeof(CardView), "-",
            propertyChanged: OnExpirationDateChanged);

    public string ExpirationDate
    {
        get => (string)GetValue(ExpirationDateProperty);
        set => SetValue(ExpirationDateProperty, value);
    }

    public static readonly BindableProperty CardValidationCodeProperty
    = BindableProperty.Create(nameof(CardValidationCode),
        typeof(string), typeof(CardView), "-",
        propertyChanged: OnCardValidationCodeChanged);

    public string CardValidationCode
    {
        get => (string)GetValue(CardValidationCodeProperty);
        set => SetValue(CardValidationCodeProperty, value);
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
    private string cardImage;

    private bool bandeauEnabled;

    public bool BandeauEnabled
    {
        get { return bandeauEnabled; }
        set { bandeauEnabled = value; Bandeau.Opacity = value ? 1 : 0.5; }
    }

    private static void OnExpirationDateChanged(BindableObject bindable,
        object oldValue, object newValue)
    {
        if (bindable is not CardView creditCardView)
            return;

        creditCardView.SetExpirationDate();
    }

    private static void OnCardValidationCodeChanged(BindableObject bindable,
        object oldValue, object newValue)
    {
        if (bindable is not CardView creditCardView)
            return;

        creditCardView.SetCardValidationCode();
    }


    public CardView()
    {
        InitializeComponent();


        CreditCardImageLabel.Text = "\uf09d";
        CreditCardImageLabel.FontFamily = "FA6Regular";

        ExpirationDateLabel.Text = "-";

        CardValidationCodeLabel.Text = "-";
    }

    private void SetExpirationDate()
    {
        ExpirationDateLabel.Text = ExpirationDate;
    }

    private void SetCardValidationCode()
    {
        CardValidationCodeLabel.Text = CardValidationCode;
    }

    private async void OnTacheClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//sites?execution={CardNumber}");
    }
}