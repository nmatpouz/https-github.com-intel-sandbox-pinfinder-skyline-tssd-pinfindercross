using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record SummaryCall : CallBase<SummaryReply>
{
    public required string LotId { get; set; }
    public required string OperCode { get; set; }
}

