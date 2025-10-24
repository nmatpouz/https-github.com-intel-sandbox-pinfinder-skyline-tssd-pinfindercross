using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Controllers;

public interface IProductDetailQueryController
{
    Task<ProductDetailReply> GetProductDetailData(string pkg, string prgnm, string tiuID);
}

public class ProductDetailQueryController : IProductDetailQueryController
{
    private readonly IMediator _mediator;

    public ProductDetailQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<ProductDetailReply> GetProductDetailData(string pkg, string prgnm, string tiuID)
    {
        return await _mediator.Send(new ProductDetailCall() { Pkg = pkg,  Prgnm = prgnm, TiuID = tiuID});
    }
}
