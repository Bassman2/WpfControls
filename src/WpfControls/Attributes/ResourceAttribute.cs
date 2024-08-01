using System.Diagnostics.CodeAnalysis;

namespace WpfControls.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class ResourceAttribute(string resource = "") : Attribute
{    
    public static readonly ResourceAttribute Default = new();

    public virtual string Resource { get => ResourceValue; }

    protected string ResourceValue { get; set; } = resource;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ResourceAttribute other && other.Resource == Resource;
    public override int GetHashCode() => Resource?.GetHashCode() ?? 0;
    
    public override bool IsDefaultAttribute() => Equals(Default);
}
