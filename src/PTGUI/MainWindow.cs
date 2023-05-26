using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PTG;

namespace PTGUI;

public class MainWindow : BaseWindow
{
    private Panel? deviceInfoPanel;
    private ListBox? packagesList;
    public MainWindow()
    {
        Title = "PTGUI";
        MinHeight = 700;
        MinWidth = 1000;
        InitializeUi();
    }

    void InitializeUi()
    {
        var grid = new Grid()
        {
            Name = "MainGrid",
            ColumnDefinitions = new ColumnDefinitions("1*, 5*"),
            ShowGridLines = true
        };
        this.Content = grid;

        var sideBar = new StackPanel()
        {
            Name = "SidePanel",
            [Grid.ColumnProperty] = 0
        };
        grid.Children.Add(sideBar);

        var devicesList = new ComboBox()
        {
            Name = "devicesList"
        };
        sideBar.Children.Add(devicesList);
        devicesList.SelectionChanged += SelectedDeviceChanged;
        devicesList.Tapped += tapped;

        deviceInfoPanel = new Panel()
        {
            Name = "deviceInfoPanel",
            Background = Avalonia.Media.Brushes.DarkBlue,
            MinWidth = 150,
            MinHeight = 150
        };
        sideBar.Children.Add(deviceInfoPanel);


        var mainContent = new StackPanel()
        {
            Name = "mainContenet",
            [Grid.ColumnProperty] = 1
        };
        grid.Children.Add(mainContent);

        packagesList = new ListBox()
        {
            Name = "packagesList",
            SelectionMode = SelectionMode.Multiple,
            MaxHeight = 600
        };
        mainContent.Children.Add(packagesList);

        var uninstallButton = new Button()
        {
            Content = "uninstall",
        };
        mainContent.Children.Add(uninstallButton);
        uninstallButton.Click += uninstallButton_Clicked;
    }

    private async void uninstallButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("uninstalling...");
        var children = ((Grid)this.Content).GetVisualChildren();
        var deviceList = FindControl<ComboBox>(children, "devicesList");
        // var deviceList = this.FindNameScope().Find<ComboBox>("devicesList");

        if (deviceList is null || deviceList.SelectedItem is null) return;
        if (packagesList is null || packagesList.SelectedItems.Count <= 0) return;

        var packages = packagesList.SelectedItems.Cast<string>();

        PopupWindow confirmationPopup = new(packages);
        await confirmationPopup.ShowDialog(this);

        if(!confirmationPopup.isConfirmed) return;

        PlatformTools.Adb.uninstallPackages((string)deviceList.SelectedItem, packages);
    }

    private void tapped(object? sender, RoutedEventArgs e)
    {
        ((ComboBox)sender!).Items = PlatformTools.Adb.GetDevices();
    }

    private void SelectedDeviceChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count <= 0)
            return;
        Console.WriteLine(e.AddedItems[0]);
        SetDeviceInfo((string)e.AddedItems[0]!);
        packagesList!.Items = PlatformTools.Adb.Get3rdApps((string)e.AddedItems[0]!);
    }

    void SetDeviceInfo(string deviceName)
    {
        /*
        - company
        - phone name
        - code name
        - OS?
        - cpu architecture
        */
        var deviceInfo = PlatformTools.Adb.GetDeviceIGeneralnfo(deviceName);
        var text = new TextBlock()
        {
            Margin = new Avalonia.Thickness(15)
        };
        foreach (var info in deviceInfo)
        {
            text.Text = text.Text + "\n " + info;
        }
        deviceInfoPanel!.Children.Add(text);
    }
}
