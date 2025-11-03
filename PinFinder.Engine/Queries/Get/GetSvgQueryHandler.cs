using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Database;

namespace PinFinder.Core.Queries.Get;

public class GetSvgQueryHandler : IRequestHandler<SvgCall, SvgReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetSvgQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<SvgReply> Handle(SvgCall request, CancellationToken cancellationToken)
    {
        return  _contextFactory.QuerySvg(request.TiuDesignID, request.MUDesignID, request.Pkg, request.Prgnm, request.TiuID);
    }
}