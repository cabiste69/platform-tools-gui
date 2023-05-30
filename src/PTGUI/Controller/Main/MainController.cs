using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PTGUI.Extensions;
using PTGUI.View.ConfirmationPopup;
using PTGUI.View.Main;

namespace PTGUI.Controller.Main;

public class MainController
{
    private readonly IVisual _view;
    public MainController(MainView view)
    {
        _view = view as IVisual;
        LoadConnectedDevices(null, null);
    }

    public void SelectedDeviceChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count <= 0)
            return;

        SetDeviceInfo((string)e.AddedItems[0]!);

        var packagesList = _view.FindControl<ListBox>("packagesList");
        packagesList!.Items = PlatformTools.Adb.Get3rdApps((string)e.AddedItems[0]!);
    }

    public void LoadConnectedDevices(object? sender, RoutedEventArgs? e)
    {
        if (sender is null)
            sender = _view.FindControl<ComboBox>("devicesList");

        ((ComboBox)sender!).Items = PlatformTools.Adb.GetDevices();
    }

    private void SetDeviceInfo(string deviceName)
    {
        /*
        - company
        - phone name
        - code name
        - OS?
        - cpu architecture
        */
        var deviceInfo = PlatformTools.Adb.GetDeviceIGeneralnfo(deviceName);

        TextBlock text = new()
        {
            Margin = new Avalonia.Thickness(10),
        };

        foreach (var info in deviceInfo)
        {
            text.Text = text.Text + "\n " + info;
        }

        var deviceInfoPanel = ((IVisual)_view).FindControl<Panel>("deviceInfoPanel");

        deviceInfoPanel!.Children.Add(text);
    }

    public async void uninstallButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("uninstalling...");

        var deviceList = _view.FindControl<ComboBox>("devicesList");
        if (deviceList is null || deviceList.SelectedItem is null) return;

        var packagesList = _view.FindControl<ListBox>("packagesList");
        if (packagesList is null || packagesList.SelectedItems.Count <= 0) return;

        var packages = packagesList.SelectedItems.Cast<string>();

        PopupWindow confirmationPopup = new(packages);
        await confirmationPopup.ShowDialog((Window)((Control)_view).Parent!);

        if (!confirmationPopup.isConfirmed) return;

        PlatformTools.Adb.uninstallPackages((string)deviceList.SelectedItem, packages);
    }
}
