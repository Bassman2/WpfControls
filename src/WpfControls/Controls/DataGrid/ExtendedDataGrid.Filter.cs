namespace WpfControls.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    public static readonly DependencyProperty AutoFilterProperty =
        DependencyProperty.Register("AutoFilter", typeof(bool), typeof(ExtendedDataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(
                (d, e) => ((ExtendedDataGrid)d).OnAutoFilterPropertyChanged((bool)e.NewValue))));

    public bool AutoFilter
    {
        get => (bool)GetValue(AutoFilterProperty);
        set => SetValue(AutoFilterProperty, value);
    }

    private void OnAutoFilterPropertyChanged(bool value)
    { }

    //protected override void OnInitialized(EventArgs e)
    //{
    //    base.OnInitialized(e);
    //}

    //protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    //{
    //    base.OnItemsChanged(e);
    //}

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

        if (newValue is ICollectionView collectionView)
        {
            // Fill columns
            foreach (IFilterColumn column in this.Columns.Where(c => c is IFilterColumn).Cast<IFilterColumn>())
            {
                column.ItemsSourceChanged(oldValue, newValue);
            }

            // activate filtering
            collectionView.Filter = DoFilter;
        }
        else
        {
            throw new Exception("ItemsSource must be inherit from ICollectionView to use filtering");
        }
    }

    private bool DoFilter(object obj)
    {
        bool res = true;
        foreach (IFilterColumn column in this.Columns.Where(c => c is IFilterColumn).Cast<IFilterColumn>())
        {
            res &= column.Filter(obj);
        }
        return res;

    }

    public void RefreshFilter()
    {
        if (this.ItemsSource is ICollectionView collectionView)
        {
            collectionView.Refresh();
        }
    }

   

    //private void OnValueChanged(object? sender, EventArgs e)
    //{
    //    // Do some stuff here..
    //}

    ////private ICollectionView collectionView;

    //private void Update()
    //{
    //    bool hasFilters = this.Columns.Where(c => c is IFilterColumn).Any();
    //    if (AutoFilter && hasFilters)
    //    {
    //        if (ItemsSource is ICollectionView collection)
    //        {
    //            //collectionView = collection;
    //            collection.Filter = DoFilter;
    //            collection.Refresh();
    //        }

    //    }

    //}

    //private bool DoFilter(object obj)
    //{
    //    //foreach (this.Columns.Where(c => c is IFilterColumn))
    //    //ItemViewModel item = (ItemViewModel)obj;
    //    //return (((int)item.FilterEnum) & FilterEnumValue) != 0 && (((int)item.FilterListValue) & FilterListValue) != 0;
    //    return true;

    //}


    //#region Binding Helper
    //private static Type? GetBindingType(BindingBase binding, object obj)
    //{
    //    string propertyName = ((Binding)binding).Path.Path;
    //    PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
    //    return property?.PropertyType;
    //}

    //private static object? GetBindingValue(BindingBase binding, object obj)
    //{
    //    string propertyName = ((Binding)binding).Path.Path;
    //    PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
    //    return property?.GetValue(obj);
    //}

    //private static string? GetBindingText(BindingBase binding, object obj)
    //{
    //    string propertyName = ((Binding)binding).Path.Path;
    //    PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
    //    return property?.GetValue(obj)?.ToString();
    //}

    //private void SetBindingHandler(BindingBase binding, object obj, EventHandler handler)
    //{
    //    string propertyName = ((Binding)binding).Path.Path;
    //    PropertyDescriptor? property = TypeDescriptor.GetProperties(obj).Find(propertyName, false);
    //    property?.AddValueChanged(obj, handler);
    //}

    //#endregion

}
