using Avalonia.Controls;
using Avalonia.Interactivity;
using PTGUI.Interfaces;

namespace PTGUI.View.ConfirmationPopup;

public class PopupWindow : Window, IView
{
    public bool isConfirmed { get; set; } = false;
    private IEnumerable<string> _packages { get; set; }

    public PopupWindow(IEnumerable<string> packages)
    {
        _packages = packages;
        Title = "Confirmation";
        Height = 700;
        Width = 1000;
        InitializeUi();
    }

    public void InitializeUi()
    {
        StackPanel sp = new();
        this.Content = sp;

        ItemsRepeater packagesList = new()
        {
            Name = "packagesList",
            MaxHeight = 600,
            Items = _packages,
        };
        sp.Children.Add(packagesList);

        Button buttonYes = new()
        {
            Content = "uninstall",
        };
        buttonYes.Click += ClickedYes;
        sp.Children.Add(buttonYes);

        Button buttonNo = new()
        {
            Content = "cancel"
        };
        buttonNo.Click += CLickedNo;
        sp.Children.Add(buttonNo);
    }

    private void ClickedYes(object? sender, RoutedEventArgs e)
    {
        isConfirmed = true;
        this.Close();
    }

    private void CLickedNo(object? sender, RoutedEventArgs e) => this.Close();
}
