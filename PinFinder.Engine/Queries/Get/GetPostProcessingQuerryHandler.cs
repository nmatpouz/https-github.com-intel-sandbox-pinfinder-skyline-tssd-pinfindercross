using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Database;

namespace PinFinder.Core.Queries.Get;

public class GetPostProcessingQueryHandler : IRequestHandler<PostProcessingCall, PostProcessingReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetPostProcessingQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<PostProcessingReply> Handle(PostProcessingCall request, CancellationToken cancellationToken)
    {
        return  _contextFactory.QueryPostProcessing(request.Config, request.LotId, request.OperCode, request.SummaryName);
    }
}