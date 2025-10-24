namespace PinFinder.Core.Domain.Commands.Replies;

public sealed record UnitReply : ReplyBase
{
    public required Dictionary<string, UnitHeader> Units { get; set; }
}

public sealed record UnitHeader
{
    public string? Site { get; set; }
    public string? TestNameAccuracy { get; set; }
    public string? Start { get; set; }
    public string? TiuId { get; set; }
    public string? End { get; set; }
    public string? Sum { get; set; }
    public string? ProductNum { get; set; }
    public string? Ult { get; set; }
    public string? Fbin { get; set; }
    public string? VisualId { get; set; }
    public string? EBin { get; set; }
    public required List<TestNameInfo> Tests { get; set; }  // Change from Dictionary to List
}

public sealed record TestNameInfo
{
    public string? Tname { get; set; }
    public string? Die { get; set; }
    public string? Connector { get; set; }
    public string? FailCycle { get; set; }
    public string? PinName { get; set; }
    public string? TiuPinName { get; set; }
    public string? Measure { get; set; }
    public string? FailPattern { get; set; }
    public string? Channel { get; set; }
    public string? FailVector { get; set; }
}