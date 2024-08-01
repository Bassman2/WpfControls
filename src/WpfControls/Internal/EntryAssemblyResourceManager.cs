namespace WpfControls.Internal;

internal static class EntryAssemblyResourceManager
{
    private static readonly ResourceManager? resourceManager;

    static EntryAssemblyResourceManager()
    {
        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Assembly assembly = Assembly.GetEntryAssembly()!;
            string? name = assembly.DefinedTypes.SingleOrDefault(t => t.Name == "Resources")?.FullName!;
            if (name is not null)
            {
                resourceManager = new ResourceManager(name, assembly);
            }
        }
    }

    public static string? GetString(object item)
    {
        if (resourceManager is not null && item.GetType().GetField(item.ToString()!)?.GetCustomAttributes<ResourceAttribute>().FirstOrDefault() is ResourceAttribute resourceAttribute)
        {
            return resourceManager.GetString(resourceAttribute.Resource) ?? item.ToString();
        }
        return item.ToString();
    }
}
