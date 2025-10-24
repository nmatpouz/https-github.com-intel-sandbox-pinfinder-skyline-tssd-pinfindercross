using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Domain.Commands.Calls;

public sealed record ProductDetailCall : CallBase<ProductDetailReply>
{
    public required string Pkg { get; set; }
    public required string Prgnm { get; set; }
    public required string TiuID { get; set; }
}

