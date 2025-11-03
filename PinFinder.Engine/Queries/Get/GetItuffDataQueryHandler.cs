using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Database;

namespace PinFinder.Core.Queries.Get;

public class GetHeaderQueryHandler : IRequestHandler<HeaderCall, HeaderReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetHeaderQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<HeaderReply> Handle(HeaderCall request, CancellationToken cancellationToken)
    {
        return  _contextFactory.QueryHeader(request.LotId, request.OperCode, request.SummaryName);
    }
}

public class GetUnitQueryHandler : IRequestHandler<UnitCall, UnitReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetUnitQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<UnitReply> Handle(UnitCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QueryUnit(request.LotId, request.OperCode, request.SummaryName, request.Site);

    }
}
public class GetSummaryQueryHandler : IRequestHandler<SummaryCall, SummaryReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetSummaryQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<SummaryReply> Handle(SummaryCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QuerySummary(request.LotId, request.OperCode);

    }
}
public class GetSiteQueryHandler : IRequestHandler<SiteCall, SiteReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetSiteQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<SiteReply> Handle(SiteCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QuerySite(request.LotId, request.OperCode, request.Site);

    }
}
public class GetTiuIdUnitDataQueryHandler : IRequestHandler<TiuIdCall, UnitReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetTiuIdUnitDataQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<UnitReply> Handle(TiuIdCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QueryTiuIdUnitData(request.TiuId, request.Site, request.DataNumberOfDay);

    }
}
