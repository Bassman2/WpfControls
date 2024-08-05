namespace WpfControls.Controls;

// https://github.com/dotnet/wpf/tree/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls

public class DataGridColorColumn : DataGridBoundColumn
{
    [DebuggerDisplay("ColorName {Color} {Name}")]
    private class ColorName 
    {   
        public ColorName(PropertyInfo propertyInfo) 
        {
            Name = propertyInfo.Name;
            //Color = ((Color?)propertyInfo.GetValue(null)).GetValueOrDefault(Colors.Transparent);
            Color = (Color)propertyInfo.GetValue(null)!;
            Brush = new SolidColorBrush(Color);
        }

        public string Name { get; }
        public Color Color { get; } 
        public Brush Brush { get; } 
    }

    //private readonly static List<ColorName> colorNames = typeof(Colors).GetProperties().Select(p => new ColorName { Color = ((Color?)p.GetValue(null)).GetValueOrDefault(Colors.Transparent), Name = p.Name }).ToList();
    //private readonly static List<Color> colorList = typeof(Colors).GetProperties().Select(p => ((Color?)p.GetValue(null)).GetValueOrDefault()).ToList();

    private readonly static List<ColorName> colorNames;
    //private readonly static List<Color> colorList;

    static DataGridColorColumn()
    {
        var colors = typeof(Colors).GetProperties();
        colorNames = colors.Select(p => new ColorName(p)).ToList();
        //colorList = colors.Select(p => ((Color?)p.GetValue(null)).GetValueOrDefault()).ToList();
    }



    private static string ToColorName(Color color) => colorNames.FirstOrDefault(c => c.Color == color)?.Name ?? "Unknown";

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        // Color Rectangle                

        //Color color = (Color)this.Binding.GetBindingValue(dataItem)!;

        //ColorName colorName = colorNames.First(c => c.Color == color);
        
        
        Rectangle rectangle = new() { Width = 14, Height = 14, Margin = new Thickness(6, 0, 2, 0) };
        Binding rectBinding = new Binding() { Path = ((Binding)this.Binding).Path, Converter = new ColorBrushConverter() };
        BindingOperations.SetBinding(rectangle, Rectangle.FillProperty, rectBinding);

        // Color Name
        TextBlock textBlock = new();// { Text = colorName.Name };
        Binding textBinding = new Binding() { Path = ((Binding)this.Binding).Path, Converter = new ColorNameConverter() };
        BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, textBinding);

        StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };
        stackPanel.Children.Add(rectangle);
        stackPanel.Children.Add(textBlock);
        return stackPanel;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        ComboBox comboBox = new() { ItemsSource = colorNames };
        comboBox.SetBinding(ComboBox.SelectedValueProperty, this.Binding);
        comboBox.SetValue(ComboBox.SelectedValuePathProperty, "Color");


        DataTemplate dataTemplate = new(typeof(ComboBox));

        FrameworkElementFactory stackPanel = new(typeof(StackPanel));
        stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

        FrameworkElementFactory rectangle = new(typeof(System.Windows.Shapes.Rectangle));
        rectangle.SetBinding(System.Windows.Shapes.Rectangle.FillProperty, new Binding("Brush"));
        rectangle.SetValue(System.Windows.Shapes.Rectangle.WidthProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.HeightProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.MarginProperty, new Thickness(0, 0, 2, 0));
        stackPanel.AppendChild(rectangle);

        FrameworkElementFactory textBlock = new(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")); // { Converter = new ColorToNameConverter() });

        stackPanel.AppendChild(textBlock);

        dataTemplate.VisualTree = stackPanel;

        comboBox.ItemTemplate = dataTemplate;
        return comboBox;
    }

    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return colorNames.First(c => c.Color == color).Brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ColorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return colorNames.First(c => c.Color == color).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    //public class ColorNameBehavior : Behavior<TextBlock>
    //{
    //    public static readonly DependencyProperty ColorProperty =
    //        DependencyProperty.Register("Color", typeof(Color), typeof(ColorNameBehavior),
    //        new PropertyMetadata(Colors.Yellow, (d, e) => ((ColorNameBehavior)d).AssociatedObject.Text = DataGridColorColumn.ToColorName((Color)e.NewValue)));

    //    public Color Color
    //    {
    //        get => (Color)GetValue(ColorProperty);
    //        set => SetValue(ColorProperty, value);
    //    }
    //}
}


