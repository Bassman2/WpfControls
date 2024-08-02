namespace WpfControls.Controls;

public abstract class DataGridAutoFilterColumn : DataGridFilterColumn
{
    protected List<FilterViewModel>? checkedFilters;

    public override void OnChecked(object sender, RoutedEventArgs e)
    {
        base.OnChecked(sender, e);

        checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();

        if (this.DataGridOwner is ExtendedDataGrid dataGrid)
        {
            dataGrid.RefreshFilter();
        }
    }
    
    public abstract void FillColumn(ICollectionView items);

    public abstract bool Filter(object obj);

    
    #region Binding Helper

    protected Type BindingType(IEnumerable newValue)
    {
        object obj = newValue.Cast<object>().First();
        return this.Binding.GetBindingType(obj)!;
    }

    

    #endregion
}
