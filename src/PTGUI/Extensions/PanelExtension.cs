using Avalonia;
using System.Reflection;
using Avalonia.VisualTree;

namespace PTGUI.Extensions;
public static class PanelExtension
{
    public static T? FindControl<T>(this IVisual parent, string name) where T : class
    {
        var children = parent.GetVisualChildren();
        foreach (var child in children)
        {
            if (child.GetType() == typeof(T) && ((StyledElement)child).Name == name)
                return (T)child;

            // https://stackoverflow.com/a/10718451/12429279
            if (typeof(IVisual).GetTypeInfo().IsAssignableFrom(child.GetType()))
            {
                var grandChild = FindControl<T>(((IVisual)child), name);
                if (grandChild is not null)
                    return grandChild;
            }
        }
        return null;
    }
}
