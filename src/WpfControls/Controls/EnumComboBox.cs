namespace WpfControls.Controls;

public class EnumComboBox : ComboBox
{
    //private static readonly ResourceManager resourceManager = Rails.Properties.Resources.ResourceManager;

    private static readonly ResourceManager? resourceManager; // = new ResourceManager("Rails.Properties.Resources", Assembly.GetEntryAssembly()!);

    static EnumComboBox()
    {
        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Assembly ass = Assembly.GetEntryAssembly()!;
            //var x = ass.ExportedTypes.SingleOrDefault(t => t.Name == "Resources");
            string? name = ass.ExportedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
            if (name is not null)
            {
                resourceManager = new ResourceManager(name, ass);
            }
        }
    }

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

        List<EnumerationMember> list = [];

        foreach (var item in Enum.GetValues(enumType))
        {
            string resName = $"Enum{enumType.Name}{item}";
            string? description = resourceManager?.GetString(resName);
            if (description == null) 
            {
                string name = item.ToString()!;
                var descriptionAttribute = enumType.GetField(name)?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (descriptionAttribute != null) 
                {
                    description = descriptionAttribute.Description;
                }
                else
                {
                    description = item.ToString()!;
                }
            }

            list.Add(new EnumerationMember(item, description)); 
        }
        this.ItemsSource = list;

    }

    public class EnumerationMember(object value, string description)
    {
        public string Description { get; } = description;
        public object Value { get; } = value;
    }
}
