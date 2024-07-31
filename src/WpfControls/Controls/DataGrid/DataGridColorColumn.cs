using System.Windows.Media.Animation;

namespace WpfControls.Controls;

// https://github.com/dotnet/wpf/tree/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls

public class DataGridColorColumn : DataGridBoundColumn
{
    private class ColorName { public Color Color; public required string Name; }
    private readonly static List<ColorName> ColorNames = typeof(Colors).GetProperties().Select(p => new ColorName { Color = ((Color?)p.GetValue(null)).GetValueOrDefault(Colors.Transparent), Name = p.Name }).ToList();
    private readonly static List<Color> ColorList = typeof(Colors).GetProperties().Select(p => ((Color?)p.GetValue(null)).GetValueOrDefault()).ToList();

    private static string ToColorName(Color color) => ColorNames.FirstOrDefault(c => c.Color == color)?.Name ?? "Unknown";

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        // Color Rectangle                
        SolidColorBrush brush = new();
        BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, this.Binding);
        System.Windows.Shapes.Rectangle rectangle = new() { Fill = brush, Width = 14, Height = 14, Margin = new Thickness(6, 0, 2, 0) };

        // Color Name
        TextBlock textBlock = new();
        ColorNameBehavior colorNameBehavior = new();
        BindingOperations.SetBinding(colorNameBehavior, ColorNameBehavior.ColorProperty, this.Binding);
        Interaction.GetBehaviors(textBlock).Add(colorNameBehavior);

        StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };
        stackPanel.Children.Add(rectangle);
        stackPanel.Children.Add(textBlock);
        return stackPanel;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        ComboBox comboBox = new() { ItemsSource = ColorList };
        comboBox.SetBinding(ComboBox.SelectedItemProperty, this.Binding);

        DataTemplate dataTemplate = new(typeof(ComboBox));

        FrameworkElementFactory stackPanel = new(typeof(StackPanel));
        stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

        FrameworkElementFactory rectangle = new(typeof(System.Windows.Shapes.Rectangle));
        rectangle.SetBinding(System.Windows.Shapes.Rectangle.FillProperty, new Binding(".") { Converter = new ColorToBrushConverter() });
        rectangle.SetValue(System.Windows.Shapes.Rectangle.WidthProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.HeightProperty, 14.0);
        rectangle.SetValue(System.Windows.Shapes.Rectangle.MarginProperty, new Thickness(0, 0, 2, 0));
        stackPanel.AppendChild(rectangle);

        FrameworkElementFactory textBlock = new(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding(".") { Converter = new ColorToNameConverter() });

        stackPanel.AppendChild(textBlock);

        dataTemplate.VisualTree = stackPanel;

        comboBox.ItemTemplate = dataTemplate;
        return comboBox;
    }

    public class ColorNameBehavior : Behavior<TextBlock>
    {
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorNameBehavior),
            new PropertyMetadata(Colors.Yellow, (d, e) => ((ColorNameBehavior)d).AssociatedObject.Text = DataGridColorColumn.ToColorName((Color)e.NewValue)));

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }

    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new SolidColorBrush((Color)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class ColorToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => DataGridColorColumn.ToColorName((Color)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}


