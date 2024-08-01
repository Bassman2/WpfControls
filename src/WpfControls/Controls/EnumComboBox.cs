using WpfControls.Internal;

namespace WpfControls.Controls;

public class EnumComboBox : ComboBox
{
    ////private static readonly ResourceManager resourceManager = Rails.Properties.Resources.ResourceManager;

    //private static readonly ResourceManager? resourceManager; // = new ResourceManager("Rails.Properties.Resources", Assembly.GetEntryAssembly()!);

    //static EnumComboBox()
    //{
    //    if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
    //    {
    //        Assembly ass = Assembly.GetEntryAssembly()!;
    //        //var x = ass.ExportedTypes.SingleOrDefault(t => t.Name == "Resources");
    //        string? name = ass.ExportedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
    //        if (name is not null)
    //        {
    //            resourceManager = new ResourceManager(name, ass);
    //        }
    //    }
    //}

    public EnumComboBox()
    {
        DisplayMemberPath = "Description";
        SelectedValuePath = "Value"; 
    }

    public static readonly DependencyProperty EnumTypeProperty =
        DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumComboBox),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback((o, e) => ((EnumComboBox)o).OnEnumTypePropertyChanged(e))));

    public Type EnumType
    {
        get => (Type)GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }
    
    private void OnEnumTypePropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        Type enumType = (Type)e.NewValue;
        if (enumType.IsEnum == false)
        {
            throw new ArgumentException("Type must be an Enum.");
        }
        this.ItemsSource = Enum.GetValues(enumType).Cast<object>().Select(i => new EnumerationMember(i, GetString(i))).ToList();
    }

    private static string GetString(object item)
    {
        FieldInfo fieldInfo = item.GetType().GetField(item.ToString()!)!;
        if (fieldInfo.GetCustomAttribute<ResourceAttribute>() is ResourceAttribute resourceAttribute)
        {
            return EntryAssemblyResourceManager.GetString(resourceAttribute.Name) ?? "";
        }
        else if (fieldInfo.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }
        else
        {
            return fieldInfo.Name;
        }
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);
    }

    public class EnumerationMember(object value, string description)
    {
        public string Description { get; } = description;
        public object Value { get; } = value;
    }
}
