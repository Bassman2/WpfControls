

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


        //if (newValue is ICollectionView collection)
        {
            object? obj = newValue.Cast<object>().FirstOrDefault();
            if (obj != null)
            {
                foreach (IFilterColumn column in this.Columns.Where(c => c is IFilterColumn))
                {
                    
                    Type type = column.Binding.GetType();
                    PropertyPath? propertyPath = (PropertyPath?)(type.GetProperty("Path")!.GetValue(column.Binding));
                    string propertyName = propertyPath!.Path;

                    var l = TypeDescriptor.GetProperties(obj);
                    var n0 = l[0];
                    var n1 = l[1];
                    var n2 = l[2];
                    var n3= l[3];
                    var n = l[propertyName];
                    var x = l.Find(propertyName, false);
                    Type enumType = n!.PropertyType;
                    //n!.AddValueChanged(obj, OnValueChanged);

                    //(()).AddValueChanged(this.AssociatedObject.DataContext, PropertyListener_ValueChanged);
                }
            }
        }
    }


    private void OnValueChanged(object sender, EventArgs e)
    {
        // Do some stuff here..
    }

    //private ICollectionView collectionView;

    private void Update()
    {
        bool hasFilters = this.Columns.Where(c => c is IFilterColumn).Any();
        if (AutoFilter && hasFilters)
        {
            if (ItemsSource is ICollectionView collection)
            {
                //collectionView = collection;
                collection.Filter = DoFilter;
                collection.Refresh();
            }

        }

    }

    private bool DoFilter(object obj)
    {
        //foreach (this.Columns.Where(c => c is IFilterColumn))
        //ItemViewModel item = (ItemViewModel)obj;
        //return (((int)item.FilterEnum) & FilterEnumValue) != 0 && (((int)item.FilterListValue) & FilterListValue) != 0;
        return true;

    }
}
