namespace WpfControls.Controls;

public interface IFilterColumn
{
    int FilterBitmask { get; }

    BindingBase Binding { get; }


    void SetFilterEnumType(Type enumType);
}
