namespace WpfControls.Controls;

public class DataGridAutoFilterEnumColumn : DataGridAutoFilterColumn
{
    public override void FillColumn(ICollectionView items)
    {
        if (items is not null)
        {
            Type type = BindingType(items);
            if (!type.IsEnum)
            {
                throw new Exception($"{nameof(DataGridAutoFilterEnumColumn)} Binding object must be an Enum");
            }

            this.filters = Enum.GetValues(type).Cast<object>().Select(e => new FilterViewModel(e)).ToList();
            this.checkedFilters = filters?.Where(f => f.IsChecked == true).ToList();
            Update();
        }
        else
        {
            this.filters = null;
            this.checkedFilters = null;
            Update();
        }
    }

    public override bool Filter(object obj)
    {
        int value = (int)this.Binding.GetBindingValue(obj)!;
        return this.checkedFilters!.Any(i => value == (int)i.Value!);
    }
}
