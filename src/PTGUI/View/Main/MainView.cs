using Avalonia.Controls;
using PTGUI.Controller.Main;
using PTGUI.Interfaces;

namespace PTGUI.View.Main;

public class MainView : Grid, IView
{
    /// <summary>
    /// shorthand for "controller"
    /// </summary>
    private readonly MainController _ctrl;

    public MainView()
    {
        _ctrl = new(this);
        InitializeUi();
    }

    public void InitializeUi()
    {
        DefineGrid();
        DrawLeftSideBar();
        DrawMainContent();
    }

    private void DefineGrid()
    {
        this.ColumnDefinitions = new ColumnDefinitions("1*, 5*");
        this.ShowGridLines = true;
    }

    private void DrawLeftSideBar()
    {
        StackPanel leftSideBar = new()
        {
            Name = "SidePanel",
            [Grid.ColumnProperty] = 0
        };
        this.Children.Add(leftSideBar);

        ComboBox devicesList = new()
        {
            Name = "devicesList"
        };
        leftSideBar.Children.Add(devicesList);
        devicesList.SelectionChanged += _ctrl.SelectedDeviceChanged;
        devicesList.Tapped += _ctrl.tapped;

        Panel deviceInfoPanel = new()
        {
            Name = "deviceInfoPanel",
            Background = Avalonia.Media.Brushes.DarkBlue,
            MinWidth = 150,
            MinHeight = 150,
        };
        leftSideBar.Children.Add(deviceInfoPanel);
    }

    private void DrawMainContent()
    {
        Grid mainContentGrid = new()
        {
            Name = "mainContentGrid",
            RowDefinitions = new RowDefinitions("10*, 1*"),
            [Grid.ColumnProperty] = 1
        };
        this.Children.Add(mainContentGrid);

        StackPanel mainContentSp = new()
        {
            Name = "mainContent",
            [Grid.RowProperty] = 0
        };
        mainContentGrid.Children.Add(mainContentSp);

        ListBox packagesList = new()
        {
            Name = "packagesList",
            SelectionMode = SelectionMode.Multiple,
            MaxHeight = 600
        };
        mainContentSp.Children.Add(packagesList);

        Button uninstallButton = new()
        {
            Name= "uninstallButton",
            Content = "uninstall",
        };
        mainContentSp.Children.Add(uninstallButton);
        uninstallButton.Click += _ctrl.uninstallButton_Clicked;
    }
}
