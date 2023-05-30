
**Setting a custom color**

`Background = new SolidColorBrush(Color.Parse("#292c34"));`

**Adding grid property to a control**

```cs
StackPanel leftSideBar = new()
    {
        Name = "SidePanel",
        // you need to add brackets for external properties (i think?)
        [Grid.ColumnProperty] = 0
    };
```
