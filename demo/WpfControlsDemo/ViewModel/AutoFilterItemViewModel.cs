namespace WpfControlsDemo.ViewModel;

public enum Enum1
{
    Empty, Active, Inactive, Removed 
}

public enum Enum2
{
    [Description("--Transparent--")]
    Transparent,
    [Description("--Red--")]
    Red,
    [Description("--Green--")]
    Green,
    [Description("--Blue--")]
    Blue,
    [Description("--White--")]
    White,
    [Description("--Black--")]
    Black
}

public partial class AutoFilterItemViewModel(string name, string description, Enum1 enum1, Enum2 enum2, string group) : ObservableObject
{
    [ObservableProperty]
    private string name = name;

    [ObservableProperty]
    private string description = description;

    [ObservableProperty]
    private Enum1 enum1 = enum1;

    [ObservableProperty]
    private Enum2 enum2 = enum2;

    [ObservableProperty]
    private string group = group;
}
