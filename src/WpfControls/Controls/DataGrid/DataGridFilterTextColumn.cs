using WpfControls.Converter;

namespace WpfControls.Controls;

public partial class DataGridFilterTextColumn : DataGridTextColumn
{
    public DataGridFilterTextColumn()
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

    private ComboBox? filterComboBox;
    private List<FilterViewModel>? filters;
    private readonly FilterViewModel allFilter = new();


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

        Update();

        DataGrid dg = DataGridOwner;
    }

    protected override void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
    {
        if (newBinding != null && newBinding is Binding binding)
        {
            binding.Converter = new DescriptionConverter();
        }
        base.OnBindingChanged(oldBinding, newBinding);
    }
    
    public void OnChecked(object sender, RoutedEventArgs e)
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
        // set filter value to trigger new filtering
        int filterValue = filters?.Where(f => f.IsChecked == true).Select(f => f.Value).Aggregate((int)0, (a, b) => (int)(a | b)) ?? 0;
        if (filterValue != this.FilterValue)
        {
            Debug.WriteLine($"FilterValue {filterValue}");
            this.FilterValue = filterValue;
        }
    }

    private void Update()
    {
        if (filterComboBox != null && filters != null)
        {
            filterComboBox.ItemsSource = filters?.Prepend(allFilter);
        }
    }

    public static readonly DependencyProperty FilterEnumProperty =
        DependencyProperty.Register("FilterEnum", typeof(Type), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterEnumChanged((Type)e.NewValue)));

    public Type FilterEnum
    {
        get => (Type)GetValue(FilterEnumProperty);
        set => SetValue(FilterEnumProperty, value);
    }

    private void OnFilterEnumChanged(Type filterEnum)
    {
        this.filters = Enum.GetValues(filterEnum).Cast<object>().Select(e => new FilterViewModel(e)).ToList();
        Update();
    }

    public static readonly DependencyProperty FilterItemsProperty =
        DependencyProperty.Register("FilterItems", typeof(IEnumerable<IFilterItem>), typeof(DataGridFilterTextColumn),
        new PropertyMetadata(null, (d, e) => ((DataGridFilterTextColumn)d).OnFilterItemsChanged((IEnumerable<IFilterItem>?)e.NewValue)));

    public IEnumerable<IFilterItem>? FilterItems
    {
        get => (IEnumerable<IFilterItem>?)GetValue(FilterItemsProperty);
        set => SetValue(FilterItemsProperty, value);
    }

    private void OnFilterItemsChanged(IEnumerable<IFilterItem>? filterItems)
    {
        this.filters = filterItems!.Select(i => new FilterViewModel(i)).ToList();
        Update();
    }

    public static readonly DependencyProperty FilterValueProperty =
        DependencyProperty.Register("FilterValue", typeof(int), typeof(DataGridFilterTextColumn),
        new FrameworkPropertyMetadata(0x7fffffff, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
       
    public int FilterValue
    {
        get => (int)GetValue(FilterValueProperty); 
        set => SetValue(FilterValueProperty, value); 
    }
   

    [DebuggerDisplay("FilterViewModel {Name}")]
    public partial class FilterViewModel : ObservableObject
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

        public bool IsAll { get; } = false;
        public int Value { get; }

        [ObservableProperty]
        public string? name;

        [ObservableProperty]
        public bool? isChecked;
    }
}
