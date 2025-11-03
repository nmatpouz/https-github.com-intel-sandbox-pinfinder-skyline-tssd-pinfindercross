namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record SvgReply : ReplyBase
{
    public required List<SvgData> SvgList { get; set; }
    public required string? Context { get; set; } = null;
}
public sealed record SvgData
{
    public string? CONNECT_NUMBER { get; set; }
    public string? X { get; set; }
    public string? Y { get; set; }
}