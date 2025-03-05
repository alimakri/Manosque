namespace MauiApp1.Components;

public partial class CardTacheView : Frame
{
    #region Execution
    public static readonly BindableProperty ExecutionProperty = BindableProperty.Create(
        nameof(Execution),
        typeof(string),
        typeof(CardTacheView),
        null,
        propertyChanged: OnExecutionChanged);

    public string Execution
    {
        get => (string)GetValue(ExecutionProperty);
        set => SetValue(ExecutionProperty, value);
    }
    private static void OnExecutionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CardTacheView cardView) return;

        //cardView.ExecutionLabel.Text = (string)newValue;
    }
    #endregion

    public CardTacheView()
    {
        InitializeComponent();
    }
}