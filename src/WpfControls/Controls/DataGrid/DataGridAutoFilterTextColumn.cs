namespace WpfControls.Controls;

public class DataGridAutoFilterTextColumn : DataGridAutoFilterColumn
{
    public override void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        if (newValue != null)
        {
            Type type = BindingType(newValue);
            if (type.Name != "String")
            {
                throw new Exception($"{nameof(DataGridAutoFilterTextColumn)} Binding object must be an string");
            }

            var l = newValue.Cast<object>().Select(o => GetBindingText(this.Binding, o) ?? string.Empty).Distinct().ToList();
                        
            this.filters = l.Select(i => new FilterViewModel(i)).ToList();
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
        string text = GetBindingText(this.Binding, obj) ?? string.Empty;
        return this.checkedFilters!.Any(c => (string)c.Value! == text);
    }    
}
