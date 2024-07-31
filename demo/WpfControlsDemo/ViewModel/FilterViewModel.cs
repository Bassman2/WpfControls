namespace WpfControlsDemo.ViewModel;

public partial class FilterViewModel : ObservableObject
{
    public FilterViewModel()
    {

        this.FilterListItems =
        [
            new FilterItem("Empty"),
            new FilterItem("Tree"),
            new FilterItem("Flower")
        ];
        this.Items =
        [
            new FilterItemViewModel("Peter", "X Peter X", Colors.Red, EnumFilter.Active, this.FilterListItems[0]),
            new FilterItemViewModel("Paul",  "X Paul X" , Colors.Blue, EnumFilter.Reserve, this.FilterListItems[1]),
            new FilterItemViewModel("Susi",  "X Susi X" , Colors.Pink, EnumFilter.Removed, this.FilterListItems[2])
        ];
        this.FilterItems = new(this.Items);
        this.FilterItems.Filter = Filter;
        this.FilterItems.Refresh();
    }

    private bool Filter(object obj)
    {
        FilterItemViewModel item = (FilterItemViewModel)obj;
        return (((int)item.FilterEnum) & FilterEnumValue) != 0 && (((int)item.FilterListValue) & FilterListValue) != 0;

    }

    [ObservableProperty]
    private ObservableCollection<FilterItemViewModel> items;

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
