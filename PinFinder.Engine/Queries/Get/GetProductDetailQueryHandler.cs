using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;
using PinFinder.Database;

namespace PinFinder.Core.Queries.Get;

public class GetProductDetailQueryHandler : IRequestHandler<ProductDetailCall, ProductDetailReply>
{
    private readonly ApplicationDbContext _contextFactory;
    public GetProductDetailQueryHandler(ApplicationDbContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<ProductDetailReply> Handle(ProductDetailCall request, CancellationToken cancellationToken)
    {
        return _contextFactory.QueryProductDetail(request.Pkg, request.Prgnm, request.TiuID);
    }
}
