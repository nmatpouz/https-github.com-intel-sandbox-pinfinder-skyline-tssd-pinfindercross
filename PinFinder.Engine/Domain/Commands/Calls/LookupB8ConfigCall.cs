using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record LookupB8ConfigCall : CallBase<LookupB8ConfigReply>
{
    public required bool Request { get; set; }
}

