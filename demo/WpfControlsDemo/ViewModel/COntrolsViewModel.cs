namespace WpfControlsDemo.ViewModel;


public enum EnumCB
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

public partial class ControlsViewModel : ObservableObject
{
    [ObservableProperty]
    private EnumCB selectedEnum = EnumCB.July;
}
