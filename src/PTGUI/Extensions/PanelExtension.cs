using System.Reflection;
using Avalonia;
using Avalonia.Controls;

namespace PTGUI.Extensions;
public static class PanelExtension
{
    public static T? FindControl<T>(this Panel parent, string name) where T : class
    {
        foreach (var child in parent.Children)
        {
            if (child.GetType() == typeof(T) && ((StyledElement)child).Name == name)
                return (T)child;

            // https://stackoverflow.com/a/10718451/12429279
            if (typeof(Panel).GetTypeInfo().IsAssignableFrom(child.GetType()))
            {
                var grandChild = FindControl<T>(((Panel)child), name);
                if (grandChild is not null)
                    return grandChild;
            }
        }
        return null;
    }
}
