using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record UnitCall : CallBase<UnitReply>
{
    public required string LotId { get; set; }
    public required string OperCode { get; set; }
    public required string SummaryName { get; set; }
    public required string Site { get; set; }
}
