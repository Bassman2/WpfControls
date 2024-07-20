
namespace WpfControlsDemo.ViewModel;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        AutoFilterItems = [
            new("Peter", "Peters Description", Enum1.Active, Enum2.Green),
            new("PaulS", "PaulS Description", Enum1.Inactive, Enum2.Green),
            new("Susie", "Susie Description", Enum1.Active, Enum2.Blue),
            new("UllyS", "UllyS Description", Enum1.Removed, Enum2.Blue),
            new("Diete", "Diete Description", Enum1.Active, Enum2.Green),
            ];

        this.FilterListItems =
        [
            new FilterItem("Empty"),
            new FilterItem("Tree"),
            new FilterItem("Flower")
        ];
        this.Items = 
        [
            new ItemViewModel("Peter", "X Peter X", Colors.Red,  EnumFilter.Active,  this.FilterListItems[0]),
            new ItemViewModel("Paul",  "X Paul X" , Colors.Blue, EnumFilter.Reserve, this.FilterListItems[1]),
            new ItemViewModel("Susi",  "X Susi X" , Colors.Pink, EnumFilter.Removed, this.FilterListItems[2])
        ];
        this.FilterItems = new ListCollectionView(this.Items);
        this.FilterItems.Filter = Filter;
        this.FilterItems.Refresh();
    }

    private bool Filter(object obj)
    {
        ItemViewModel item = (ItemViewModel)obj;
        return (((int)item.FilterEnum) & FilterEnumValue) != 0 &&  (((int)item.FilterListValue) & FilterListValue) != 0;
            
    }

    [ObservableProperty]
    private ObservableCollection<AutoFilterItemViewModel> autoFilterItems;


    [ObservableProperty]
    private ObservableCollection<ItemViewModel> items;

    [ObservableProperty]
    private ListCollectionView filterItems;

    [ObservableProperty]
    public FilterItem[] filterListItems;

    [ObservableProperty]
    public int filterEnumValue = 0x7fffffff;

    partial void OnFilterEnumValueChanged(int value) => this.FilterItems.Refresh();

    [ObservableProperty]
    public int filterListValue = 0x7fffffff;

    partial void OnFilterListValueChanged(int value) => this.FilterItems.Refresh();


    [ObservableProperty]
    private int integerSliderValue = 1;

    [ObservableProperty]
    private double doubleSliderValue = 1.0;

    [ObservableProperty]
    private int integerSpinValue = 1;

    [ObservableProperty]
    private double doubleSpinValue = 1.0;
}
