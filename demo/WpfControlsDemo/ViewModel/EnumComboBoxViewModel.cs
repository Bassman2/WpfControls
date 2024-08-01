using System;

namespace WpfControlsDemo.ViewModel;


public enum EnumName
{
    Peter,
    Paul,
    Mary,
    Anne,
    July,
    Dora,
    Sigi,
    Ulli,
    Lars
}

public enum EnumDesc
{
    [Description("Blue")]
    B,
    [Description("Red")]
    R,
    [Description("Green")]
    G,
    [Description("Yellow")]
    Y,
    [Description("Magenta")]
    M,
    [Description("Pink")]
    P,
    [Description("White")]
    W
}

public enum EnumResc
{
    [Resource("ColorBlue")]
    B,
    [Resource("ColorRed")]
    R,
    [Resource("ColorGreen")]
    G,
    [Resource("ColorYellow")]
    Y,
    [Resource("ColorMagenta")]
    M,
    [Resource("ColorPink")]
    P,
    [Resource("ColorWhite")]
    W
}

public partial class EnumComboBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private EnumName selectedEnumName = EnumName.July;

    [ObservableProperty]
    private EnumDesc selectedEnumDesc = EnumDesc.R;

    [ObservableProperty]
    private EnumResc selectedEnumResc = EnumResc.W;
}
