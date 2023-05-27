using Avalonia;
using Avalonia.Controls;
// using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace PTGUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        // {
        //     desktop.MainWindow = new MainWindow();
        // }
        var app = new Application();
        app.Run(new MainWindow());

        base.OnFrameworkInitializationCompleted();
    }
}
