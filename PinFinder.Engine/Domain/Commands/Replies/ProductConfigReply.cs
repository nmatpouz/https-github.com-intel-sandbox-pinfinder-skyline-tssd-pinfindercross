namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record ProductConfigReply : ReplyBase
{
    public required List<ProductConfig> ProductConfigList { get; set; }
}
public sealed record ProductConfig
{
    public string? Name { get; set; } = null;
    public string? PackageType { get; set; } 
    public string? PackageId { get; set; }
    public string? TestProgram { get; set; }
    public string? TIU { get; set; }
    public string? IUDesignId { get; set; } = null;
    public string? MUDesignId { get; set; }
    public string? AdditionalAccessGroup { get; set; }
    public string? PPMR { get; set; }
    public string? PPMO { get; set; }
}
