namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record HeaderReply : ReplyBase
{
    public required List<HeaderData> HeaderList { get; set; }
}
public sealed record HeaderData
{
    public string? Stpl { get; set; } = null;
    public string? Soc { get; set; } 
    public string? Env { get; set; }
    public string? LotId { get; set; }
    public string? Packg { get; set; }
    public string? Prgnm { get; set; } = null;
    public string? Prdct { get; set; }
    public string? TesterType { get; set; }
    public string? Sspec { get; set; }
    public string? Lcode { get; set; }
    public string? SysId { get; set; }
    public string? Tempr { get; set; }
    public string? Ldbid { get; set; }
    public string? Smrynam { get; set; }
    public string? PrdctName { get; set; }
    public string? UniqueFolderName { get; set; }
    public string? IuDesignId { get; set; }
    public string? MuDesignId { get; set; }
    public string? OverflowFlag { get; set; }
}
