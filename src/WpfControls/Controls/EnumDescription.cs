namespace WpfControls.Controls;

internal static class EntryAssemblyResourceManager
{
    private static readonly ResourceManager? resourceManager;

    static EntryAssemblyResourceManager()
    {
        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Assembly assembly = Assembly.GetEntryAssembly()!;
            string? name = assembly.DefinedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
            //string? name = assembly.ExportedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
            if (name is not null)
            {
                resourceManager = new ResourceManager(name, assembly);
            }
        }
    }

    public static string? GetString(object item)
    {
        //item.GetType().GetField(item.ToString()!)?.GetCustomAttributes<ResourceAttribute>();
        //if (resourceManager is not null && item.GetType().GetField(item.ToString()!)?.GetCustomAttributes(typeof(ResourceAttribute), false).FirstOrDefault() is ResourceAttribute resourceAttribute)

        if (resourceManager is not null && item.GetType().GetField(item.ToString()!)?.GetCustomAttributes<ResourceAttribute>().FirstOrDefault() is ResourceAttribute resourceAttribute)
        {
            return resourceManager.GetString(resourceAttribute.Resource) ?? item.ToString();
        }



        //string resName = $"Enum{enumType.Name}{item}";
        //string? description = resourceManager?.GetString(resName);
        //if (description == null)
        //{
        //    string name = item.ToString()!;
        //    var descriptionAttribute = enumType.GetField(name)?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        //    if (descriptionAttribute != null)
        //    {
        //        return descriptionAttribute.Description;
        //    }
        //    else
        //    {
        //        return item.ToString()!;
        //    }
        //}
        return item.ToString();
    }
}
