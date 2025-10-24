using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record ProductConfigCall : CallBase<ProductConfigReply>
{
    public required bool Request { get; set; }
}

