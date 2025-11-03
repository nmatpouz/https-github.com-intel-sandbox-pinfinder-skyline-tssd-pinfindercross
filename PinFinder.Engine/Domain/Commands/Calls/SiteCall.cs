using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record SiteCall : CallBase<SiteReply>
{
    public required string LotId { get; set; }
    public required string OperCode { get; set; }
    public required string Site { get; set; }
}

