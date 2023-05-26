using System.Collections.Generic;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace PTGUI;
public class BaseWindow : Window
{
    public T? FindControl<T>(IEnumerable<IVisual> parent, string name) where T : class
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