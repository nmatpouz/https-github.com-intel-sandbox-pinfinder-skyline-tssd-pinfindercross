using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record PostProcessingCall : CallBase<PostProcessingReply>
{
    public required string Config { get; set; }
    public required string LotId { get; set; }
    public required string OperCode { get; set; }
    public required string SummaryName { get; set; }
}

