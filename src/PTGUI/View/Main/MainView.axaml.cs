using Avalonia.Controls;
using PTGUI.Controller.Main;
using PTGUI.Interfaces;

namespace PTGUI.View.Main;

public partial class MainView : Grid, IView
{
    /// <summary>
    /// shorthand for "controller"
    /// </summary>
    private readonly MainController _ctrl;
    public int MyProperty { get; set; }

    public MainView()
    {
        InitializeComponent();

        _ctrl = new(this);
    }

    protected override void OnInitialized()
    {
        WireLeftSideBar();
        WireMainContent();
    }

    private void WireLeftSideBar()
    {
        //* Link the device list
        devicesList.SelectionChanged += _ctrl.SelectedDeviceChanged;
        devicesList.Tapped += _ctrl.LoadConnectedDevices;
    }

    private void WireMainContent()
    {
        uninstallButton.Click += _ctrl.uninstallButton_Clicked;
        
    }
}
