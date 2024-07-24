using System.Windows.Media.Converters;

namespace WpfControls.Controls;

public class DataGridAutoFilterEnumColumn : DataGridAutoFilterColumn
{
    #region IFilterColumn

    public override void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        object? obj = newValue.Cast<object>().FirstOrDefault();
        if (obj != null)
        {
            Type? type = GetBindingType(this.Binding, obj);
            if (!type!.IsEnum)
            {
                throw new Exception("DataGridAutoFilterEnumColumn Binding object must be an Enum");
            }
                
            this.filters = Enum.GetValues(type).Cast<object>().Select(e => new FilterViewModel(e)).ToList();
            this.checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();
            Update();
            
            //string? val = GetBindingText(column.Binding, obj);

            //SetBindingHandler(column.Binding, obj, OnValueChanged);
        }
    }

    public override bool Filter(object obj)
    {
        object? o = GetBindingValue(this.Binding, obj);
        string str = o?.ToString() ?? "";
        int idA = (int)o!;
        //bool res = this.checkedFilters?.Any(f => f.Value == o) ?? false;
        bool res = false;
        foreach (var checkedFilter in this.checkedFilters!)
        {
            if (idA == (int)checkedFilter.Value!)
            {
                res = true;
            }

            //string xxx = checkedFilter.Value?.ToString() ?? "";
            //if (str == xxx)
            //{
            //    res = true;
            //}
        }
        return res;
    }

    #endregion

    public override void OnChecked(object sender, RoutedEventArgs e)
    {
        base.OnChecked(sender, e);

        checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();

        //FilterViewModel fvm = (FilterViewModel)((CheckBox)sender).DataContext;

        //Debug.WriteLine($"OnChecked {fvm.Name}");

        //if (fvm.IsAll) // if All button
        //{
        //    switch (fvm.IsChecked)
        //    {
        //        case true:
        //            filters?.Where(f => f.IsChecked == false).ToList().ForEach(f => f.IsChecked = true);
        //            break;
        //        case false:
        //            filters?.Where(f => f.IsChecked == true).ToList().ForEach(f => f.IsChecked = false);
        //            break;
        //        case null:
        //            break;
        //    }
        //}
        //else
        //{
        //    // update all button state
        //    if (filters?.All(f => f.IsChecked == true) ?? false)
        //    {
        //        this.allFilter!.IsChecked = true;
        //    }
        //    else if (filters?.All(f => f.IsChecked == false) ?? false)
        //    {
        //        this.allFilter!.IsChecked = false;
        //    }
        //    else
        //    {
        //        this.allFilter!.IsChecked = null;
        //    }
        //}
        //// set filter value to trigger new filtering

        //checkedFilters = filters?.Where(f => f.IsChecked == true).Select(f => f.).q
        //int filterValue = filters?.Where(f => f.IsChecked == true).Select(f => f.Value).Aggregate((int)0, (a, b) => (int)(a | b)) ?? 0;
        //if (filterValue != this.FilterBitmask)
        //{
        //    Debug.WriteLine($"FilterValue {filterValue}");
        //    this.FilterBitmask = filterValue;

        //    if (this.DataGridOwner is ExtendedDataGrid dataGrid)
        //    {
        //        dataGrid.RefreshFilter();
        //    }

        //}
    }

    //public void SetFilterEnumType(Type enumType)
    //{
    //    this.filters = Enum.GetValues(enumType).Cast<object>().Select(e => new FilterViewModel(e)).ToList();
    //    Update();
    //}

    //private void Update()
    //{
    //    if (filterComboBox != null && filters != null)
    //    {
    //        filterComboBox.ItemsSource = filters?.Prepend(allFilter);
    //    }
    //}


    


    //[DebuggerDisplay("FilterViewModel {Name}")]
    //public partial class FilterViewModel : INotifyPropertyChanged
    //{
    //    /// <summary>
    //    /// Constructor for 'All' filter item
    //    /// </summary>
    //    public FilterViewModel()
    //    {
    //        this.IsAll = true;
    //        this.Name = "All";
    //        this.IsChecked = true;
    //    }

    //    /// <summary>
    //    /// Constructor for flag enum filter item
    //    /// </summary>
    //    public FilterViewModel(object item)
    //    {
    //        FieldInfo? fieldInfo = item.GetType().GetField(item.ToString()!);
    //        DescriptionAttribute? attribute = fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
    //        this.Name = (attribute == null ? item.ToString() : attribute.Description)!;
    //        this.Value = (int)item;
    //        this.IsChecked = true;
    //    }

    //    /// <summary>
    //    /// Constructor for IFilterItem filter item
    //    /// </summary>
    //    public FilterViewModel(IFilterItem item)
    //    {
    //        this.Name = item.Name;
    //        this.Value = item.Value;
    //        this.IsChecked = true;
    //    }

    //    public event PropertyChangedEventHandler? PropertyChanged;

    //    public bool IsAll { get; } = false;
    //    public int Value { get; }

    //    public string? Name { get; }


    //    private bool? isChecked;

    //    public bool? IsChecked
    //    {
    //        get => isChecked;
    //        set
    //        {
    //            if (isChecked != value)
    //            {
    //                isChecked = value;
    //                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
    //            }
    //        }
    //    }
    //}
}
