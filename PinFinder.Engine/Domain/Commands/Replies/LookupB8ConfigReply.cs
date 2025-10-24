namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record LookupB8ConfigReply : ReplyBase
{
    public required List<LookupB8Config> LookupB8ConfigList { get; set; }
}
public sealed record LookupB8Config
{
    public string? Binning { get; set; }
    public string? PackageId { get; set; }
    public string? TestProgram { get; set; }
    public string? TIU { get; set; }
    public string? IUDesignId { get; set; } = null;
    public string? MUDesignId { get; set; }
}
