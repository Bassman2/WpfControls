

namespace WpfControlsDemo.ViewModel;

[Flags]
public enum EnumFilter
{
    [Description("Active")]
    Active      /**/= 0b_00000000_00000000_00000000_00000001, // = 1,
    Deactivated /**/= 0b_00000000_00000000_00000000_00000010, // = 2,
    Reserve     /**/= 0b_00000000_00000000_00000000_00000100, // = 4,
    Removed     /**/= 0b_00000000_00000000_00000000_00001000, // = 8,
    Reused      /**/= 0b_00000000_00000000_00000000_00010000, // = 16
}

public class FilterItem : IFilterItem
{
    private static int value = 1;
    public FilterItem(string name)
    {
        Name = name;
        Value = value;
        value = value << 1;
    }

    public string Name { get; set; }
    public int Value { get; set; }
}

public partial class FilterItemViewModel : ObservableObject
{
    public FilterItemViewModel(string name, string description, Color color, EnumFilter filterEnum, FilterItem filterList)
    { 
        this.Name = name;
        this.Description = description;
        this.Color = color;
        this.FilterEnum = filterEnum;   
        this.FilterList = filterList.Name;
        this.FilterListValue = filterList.Value;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private Color color = Colors.Red;

    [ObservableProperty]
    private string filterList = string.Empty;

    public int FilterListValue = 0;

    [ObservableProperty]
    private EnumFilter filterEnum = EnumFilter.Active;

    [ObservableProperty]
    private string password = "xxxxxxx";

    [RelayCommand]
    public void OnButton()
    {
        MessageBox.Show("Button Pressed");
    }
}
