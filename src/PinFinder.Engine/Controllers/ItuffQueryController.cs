using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Controllers;

public interface IItuffQueryController
{
    Task<HeaderReply> GetHeaderData(string lotId,string opCode, string summaryName);
    Task<UnitReply> GetUnitData(string lotId, string opCode, string summaryName, string site);
    Task<SummaryReply> GetSummaryData(string lotId, string opCode);
    Task<SiteReply> GetSiteData(string lotId, string opCode, string Site);
    Task<UnitReply> GetTiuIdUnitData(string tiuId, string site);
}

public class ItuffQueryController : IItuffQueryController
{
    private readonly IMediator _mediator;

    public ItuffQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<HeaderReply> GetHeaderData(string lotId, string opCode, string summaryName)
    {
        return await _mediator.Send(new HeaderCall() { LotId = lotId, OperCode = opCode, SummaryName = summaryName });
    }

    public async Task<UnitReply> GetUnitData(string lotId, string opCode, string summaryName, string site)
    {
        return await _mediator.Send(new UnitCall() { LotId = lotId, OperCode = opCode, SummaryName = summaryName, Site = site });
    }
    public async Task<SummaryReply> GetSummaryData(string lotId, string opCode)
    {
        return await _mediator.Send(new SummaryCall() { LotId = lotId, OperCode = opCode});
    }
    public async Task<SiteReply> GetSiteData(string lotId, string opCode, string site)
    {
        return await _mediator.Send(new SiteCall() { LotId = lotId, OperCode = opCode, Site = site });
    }
    public async Task<UnitReply> GetTiuIdUnitData(string tiuId, string site)
    {
        return await _mediator.Send(new TiuIdCall() { TiuId = tiuId,  Site = site});
    }
}
