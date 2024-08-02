namespace WpfControls.Controls;

public class DataGridButtonColumn : DataGridBoundColumn
{
    public DataGridButtonColumn()
    {
        this.IsReadOnly = true;
    }

    public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register("Command", typeof(ICommand), typeof(DataGridButtonColumn));
    //new PropertyMetadata(null, (d, e) => ((ColorNameBehavior)d).AssociatedObject.Text = DataGridColorColumn.ToColorName((Color)e.NewValue)));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty ContentTemplateProperty =
           DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(DataGridButtonColumn));
    //new PropertyMetadata(null, (d, e) => ((ColorNameBehavior)d).AssociatedObject.Text = DataGridColorColumn.ToColorName((Color)e.NewValue)));

    private DataTemplate ContentTemplate
    {
        get => (DataTemplate)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        Button button = new();
        if (this.ContentTemplate != null)
        {
            button.ContentTemplate = this.ContentTemplate;
        }
        else
        {
            button.Content = "Edit";
        }

        if (this.Binding is not null)
        {
            button.SetBinding(Button.CommandProperty, this.Binding);
            button.CommandParameter = dataItem;
        }
        return button;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        throw new NotImplementedException();
    }
}