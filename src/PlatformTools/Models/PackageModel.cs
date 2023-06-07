namespace PlatformTools.Models;

public class PackageModel
{
    public PackageType Type { get; set; }
    public PackageStatus Status { get; set; }
    public PackageModel(PackageType type, PackageStatus status)
    {
        Type = type;
        Status = status;
    }

    public override string ToString()
    {
        return TypeToString() + ", " +StatusToString();
    }

    public enum PackageType
    {
        User,
        System
    }

    public enum PackageStatus
    {
        None,
        Enabled,
        Disabled,
        Uninstalled
    }

    private string TypeToString()
    {
        if (this.Type is PackageType.System)
            return "System";
        return "User";
    }

    private string StatusToString()
    {
        if (this.Status is PackageStatus.Enabled)
            return "Enabled";
        if (this.Status is PackageStatus.None)
            return "None";
        if (this.Status is PackageStatus.Disabled)
            return "Disabled";
        return "Uninstalled";
    }
}
