namespace WpfControls.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

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
}
