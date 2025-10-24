using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Database;

namespace PinFinder.Core.Queries.Get;

public class GetProductConfigQueryHandler : IRequestHandler<ProductConfigCall, ProductConfigReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetProductConfigQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<ProductConfigReply> Handle(ProductConfigCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QueryProductConfig(request.Request);
    }
}

public class GetLookupB8ConfigQueryHandler : IRequestHandler<LookupB8ConfigCall, LookupB8ConfigReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetLookupB8ConfigQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<LookupB8ConfigReply> Handle(LookupB8ConfigCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QueryLookupB8Config(request.Request);
    }
}
