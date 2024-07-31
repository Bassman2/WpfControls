namespace WpfControlsDemo.ViewModel;

public partial class AutoFilterViewModel : ObservableObject
{
    public AutoFilterViewModel()
    {
        AutoFilterItems = new ListCollectionView(new List<AutoFilterItemViewModel>(
            [
            new("Peter", "Peter Description", Enum1.Active,   Enum2.Green, "Group D"),
            new("PaulS", "PaulS Description", Enum1.Inactive, Enum2.Green, "Group D"),
            new("Susie", "Susie Description", Enum1.Active,   Enum2.Blue,  "Group B"),
            new("UllyS", "UllyS Description", Enum1.Removed,  Enum2.Blue,  "Group B"),
            new("Diete", "Diete Description", Enum1.Active,   Enum2.Green, "Group C"),
            new("Renat", "Renat Description", Enum1.Inactive, Enum2.Green, "Group C"),
            new("Wolfg", "Wolfg Description", Enum1.Removed,  Enum2.Blue,  "Group C"),
            new("Sabbe", "Sabbe Description", Enum1.Active,   Enum2.Green, "Group A"),
            new("AnjaS", "AnjaS Description", Enum1.Active,   Enum2.Blue,  "Group A"),
            new("Felix", "Felix Description", Enum1.Active,   Enum2.Blue,  ""),
            ]));       
    }
    
    [ObservableProperty]
    private ListCollectionView autoFilterItems;    
}
