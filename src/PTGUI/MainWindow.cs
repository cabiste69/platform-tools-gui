using Avalonia.Controls;
using Avalonia.Media;
using PTGUI.Interfaces;
using PTGUI.View.Main;

namespace PTGUI;

public class MainWindow : Window, IView
{
    public MainWindow()
    {
        Title = "APT UI";
        MinHeight = 700;
        MinWidth = 1000;
        Background = new SolidColorBrush(Color.Parse("#292c34"));
        InitializeUi();
    }

    public void InitializeUi()
    {
        this.Content = new MainView();
    }
}
