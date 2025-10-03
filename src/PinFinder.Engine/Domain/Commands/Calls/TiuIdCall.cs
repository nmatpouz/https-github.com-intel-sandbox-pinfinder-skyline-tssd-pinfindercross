using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record TiuIdCall : CallBase<UnitReply>
{
    public required string TiuId { get; set; }
    public required string Site { get; set; }
}
