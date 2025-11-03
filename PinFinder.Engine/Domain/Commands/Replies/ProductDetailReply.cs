namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record ProductDetailReply : ReplyBase
{
    public required List<ProductDetail> ProductDetailList { get; set; }
}
public sealed record ProductDetail
{
    public string? ChannelNumber { get; set; } = null;
    public string? SocketID { get; set; } 
    public string? IUNetName { get; set; }
    public string? MUNetName { get; set; }
    public string? ConnectorID { get; set; }
}
