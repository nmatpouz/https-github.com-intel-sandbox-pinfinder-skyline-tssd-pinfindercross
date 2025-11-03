using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record VersionCall : CallBase<VersionReply>
{
    public required bool Call { get; set; }
}
