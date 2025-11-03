namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record SiteReply : ReplyBase
{
    public required List<SiteData> LotList { get; set; }
}
public sealed record SiteData
{
    public string? LotID { get; set; } = null;
}
