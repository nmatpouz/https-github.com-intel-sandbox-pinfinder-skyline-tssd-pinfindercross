namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record VersionReply : ReplyBase
{
    public required string? Version { get; set; } = null;
    public required string? Summary { get; set; } = null;
    public required string? PersonID { get; set; } = null;
    public required string? Date { get; set; } = null;

}