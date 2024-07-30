namespace WpfControls.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register("Count", typeof(int), typeof(ExtendedDataGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly DependencyProperty FilteredCountProperty = 
        DependencyProperty.Register("FilteredCount", typeof(int), typeof(ExtendedDataGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int FilteredCount
    {
        get => (int)GetValue(FilteredCountProperty);
        set => SetValue(FilteredCountProperty, value);
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

        this.Count = newValue.Cast<object>().Count();

        // Fill columns
        foreach (IFilterColumn column in this.Columns.Where(c => c is IFilterColumn).Cast<IFilterColumn>())
        {
            if (newValue is not ICollectionView)
            {
                throw new ArgumentException("ItemsSource must be of type ICollectionView for filtering");
            }

            column.ItemsSourceChanged(oldValue, newValue);
        }

        // activate filtering
        if (newValue is ICollectionView collectionView)
        {
            collectionView.Filter = DoFilter;
            FilteredCount = collectionView.Cast<object>().Count();
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
            FilteredCount = collectionView.Cast<object>().Count();
        }
    }
}
