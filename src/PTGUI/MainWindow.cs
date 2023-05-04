using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace PTGUI;

public class MainWindow : Window
{
    private Panel? deviceInfoPanel;
    private ListBox? packagesList;
    public MainWindow()
    {
        Title = "hiiii";
        MinHeight = 600;
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


        var mainContenet = new StackPanel()
        {
            Name = "mainContenet",
            [Grid.ColumnProperty] = 1
        };
        grid.Children.Add(mainContenet);

        packagesList = new ListBox()
        {
            Name = "packagesList",
            SelectionMode = SelectionMode.Multiple,
            MaxHeight = 600
        };
        mainContenet.Children.Add(packagesList);

        var uninstallButton = new Button()
        {
            Content = "uninstall",
        };
        mainContenet.Children.Add(uninstallButton);
        uninstallButton.Click += uninstallButton_Clicked;
    }

    private void uninstallButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("uninstalling...");

        var children = ((Grid)this.Content).GetVisualChildren();
        var deviceList = FindControl<ComboBox>(children, "devicesList");

        if (deviceList is null || deviceList.SelectedItem is null) return;
        if (packagesList is null || packagesList.SelectedItems.Count <= 0) return;

        var packages = packagesList.SelectedItems.Cast<string>();
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

    private T? FindControl<T>(IEnumerable<IVisual> parent, string name) where T : class
    {
        foreach (IVisual? child in parent)
        {
            if (child.GetType() == typeof(T) && ((StyledElement)child).Name == name)
                return (T)child;
            // https://stackoverflow.com/a/10718451/12429279
            if (typeof(Panel).GetTypeInfo().IsAssignableFrom(child.GetType()))
            {
                var x = FindControl<T>(((Panel)child).GetVisualChildren(), name);
                if (x is not null)
                    return x;
            }
        }
        return null;
    }
}
