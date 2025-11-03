namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record PostProcessingReply : ReplyBase
{
    public required List<PostProcessingData> PostProcessingList { get; set; }
}
public sealed record PostProcessingData
{
    public string? Unit { get; set; } = null;
    public string? TestName { get; set; } = null;
    public string? FailingPin { get; set; } = null;
    public string? OutputPath { get; set; } = null;
}
