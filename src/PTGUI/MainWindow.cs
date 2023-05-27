using PTGUI.View.Main;
using Avalonia.Controls;
using PTGUI.Interfaces;

namespace PTGUI;

public class MainWindow : Window, IView
{
    public MainWindow()
    {
        Title = "APT UI";
        MinHeight = 700;
        MinWidth = 1000;
        InitializeUi();
    }

    public void InitializeUi()
    {
        this.Content = new MainView();
    }
}
