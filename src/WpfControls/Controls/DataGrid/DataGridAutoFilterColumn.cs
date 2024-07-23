namespace WpfControls.Controls;

public abstract class DataGridAutoFilterColumn : DataGridTextColumn, IFilterColumn
{
    private ComboBox? filterComboBox;
    protected List<FilterViewModel>? filters;
    protected List<FilterViewModel>? checkedFilters;
    private readonly FilterViewModel allFilter = new();

    public DataGridAutoFilterColumn()
    {
        IsReadOnly = true;
        var headerTemplate = new DataTemplate() { DataType = typeof(string) };

        var dockPanel = new FrameworkElementFactory(typeof(DockPanel));
        dockPanel.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

        var textBlock = new FrameworkElementFactory(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding("Header") { Source = this });
        textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

        var comboBox = new FrameworkElementFactory(typeof(ComboBox));
        comboBox.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);
        comboBox.SetValue(DockPanel.DockProperty, Dock.Right);
        comboBox.SetValue(ComboBox.WidthProperty, 16.0);
        comboBox.SetValue(ComboBox.IsEnabledProperty, true);
        comboBox.SetBinding(ComboBox.DataContextProperty, this.Binding);
        comboBox.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));

        dockPanel.AppendChild(textBlock);
        dockPanel.AppendChild(comboBox);

        headerTemplate.VisualTree = dockPanel;
        HeaderTemplate = headerTemplate;
    }

    public void OnLoaded(object sender, RoutedEventArgs e)
    {
        filterComboBox = (ComboBox)sender;

        DataTemplate dataTemplate = new(typeof(ComboBox));
        FrameworkElementFactory checkBox = new(typeof(CheckBox));
        checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked"));
        checkBox.SetBinding(CheckBox.ContentProperty, new Binding("Name"));
        checkBox.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(OnChecked));
        checkBox.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(OnChecked));
        checkBox.AddHandler(CheckBox.IndeterminateEvent, new RoutedEventHandler(OnChecked));
        dataTemplate.VisualTree = checkBox;
        filterComboBox.ItemTemplate = dataTemplate;

        
    }

    public virtual void OnChecked(object sender, RoutedEventArgs e)
    {
        FilterViewModel fvm = (FilterViewModel)((CheckBox)sender).DataContext;

        Debug.WriteLine($"OnChecked {fvm.Name}");

        if (fvm.IsAll) // if All button
        {
            switch (fvm.IsChecked)
            {
            case true:
                filters?.Where(f => f.IsChecked == false).ToList().ForEach(f => f.IsChecked = true);
                break;
            case false:
                filters?.Where(f => f.IsChecked == true).ToList().ForEach(f => f.IsChecked = false);
                break;
            case null:
                break;
            }
        }
        else
        {
            // update all button state
            if (filters?.All(f => f.IsChecked == true) ?? false)
            {
                this.allFilter!.IsChecked = true;
            }
            else if (filters?.All(f => f.IsChecked == false) ?? false)
            {
                this.allFilter!.IsChecked = false;
            }
            else
            {
                this.allFilter!.IsChecked = null;
            }
        }

        if (this.DataGridOwner is ExtendedDataGrid dataGrid)
        {
            dataGrid.RefreshFilter();
        }
    }

    protected void Update()
    {
        if (filterComboBox != null && filters != null)
        {
            filterComboBox.ItemsSource = filters?.Prepend(allFilter);
        }
    }

    #region IFilterColumn

    public abstract void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue);

    public abstract bool Filter(object obj);

    #endregion

    #region Binding Helper

    protected static Type? GetBindingType(BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.PropertyType;
    }

    protected static object? GetBindingValue(BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.GetValue(obj);
    }

    protected static string? GetBindingText(BindingBase binding, object obj)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        return property?.GetValue(obj)?.ToString();
    }

    protected static void SetBindingHandler(BindingBase binding, object obj, EventHandler handler)
    {
        string propertyName = ((Binding)binding).Path.Path;
        PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        property?.AddValueChanged(obj, handler);
    }

    #endregion

    #region FilterViewModel

    [DebuggerDisplay("FilterViewModel {Name}")]
    public class FilterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for 'All' filter item
        /// </summary>
        public FilterViewModel()
        {
            this.IsAll = true;
            this.Name = "All";
            this.IsChecked = true;
        }

        /// <summary>
        /// Constructor for flag enum filter item
        /// </summary>
        public FilterViewModel(object item)
        {
            FieldInfo? fieldInfo = item.GetType().GetField(item.ToString()!);
            DescriptionAttribute? attribute = fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            this.Name = (attribute == null ? item.ToString() : attribute.Description)!;
            this.Value = (int)item;
            this.IsChecked = true;
        }

        /// <summary>
        /// Constructor for IFilterItem filter item
        /// </summary>
        public FilterViewModel(IFilterItem item)
        {
            this.Name = item.Name;
            this.Value = item.Value;
            this.IsChecked = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsAll { get; } = false;
        public object? Value { get; }

        public string? Name { get; }


        private bool? isChecked;

        public bool? IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }
    }

    #endregion
}
