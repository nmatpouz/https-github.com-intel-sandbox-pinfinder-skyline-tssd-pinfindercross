using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record SvgCall : CallBase<SvgReply>
{
    public required string TiuDesignID { get; set; }
    public required string MUDesignID { get; set; }
    public required string Pkg { get; set; }
    public required string Prgnm { get; set; }
    public required string TiuID { get; set; }
}

