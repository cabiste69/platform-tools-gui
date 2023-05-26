using System.Globalization;
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using PTGUI;

namespace PTG;

public class PopupWindow : BaseWindow
{
    private IEnumerable<string> _packages { get; set; }
    public bool isConfirmed { get; set; } = false;
    public PopupWindow(IEnumerable<string> packages)
    {
        _packages = packages;
        Title = "Confirmation";
        Height = 700;
        Width = 1000;
        InitializeUi();
    }

    private void InitializeUi()
    {
        StackPanel sp = new();
        this.Content = sp;

        var packagesList = new ItemsRepeater()
        {
            Name = "packagesList",
            MaxHeight = 600,
            Items = _packages,
        };
        sp.Children.Add(packagesList);

        var buttonYes = new Button()
        {
            Content = "uninstall",
        };
        buttonYes.Click += ClickedYes;
        sp.Children.Add(buttonYes);


        var buttonNo = new Button()
        {
            Content = "cancel"
        };
        buttonNo.Click += CLickedNo;
        sp.Children.Add(buttonNo);
    }

    private void CLickedNo(object? sender, RoutedEventArgs e) => this.Close();

    private void ClickedYes(object? sender, RoutedEventArgs e)
    {
        isConfirmed = true;
        this.Close();
    }
}
