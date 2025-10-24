namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record SummaryReply : ReplyBase
{
    public required List<SummaryData> SummaryList { get; set; }
}
public sealed record SummaryData
{
    public string? Summary { get; set; } = null;
}
