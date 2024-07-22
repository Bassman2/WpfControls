namespace WpfControls.Controls;

public interface IFilterColumn
{
    void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue);

    bool Filter(object obj);

    //int FilterBitmask { get; }

    //BindingBase Binding { get; }


    //void SetFilterEnumType(Type enumType);
}
