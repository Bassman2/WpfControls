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
        object? o = GetBindingValue(this.Binding, obj);
        //string str = o?.ToString() ?? "";
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
}
