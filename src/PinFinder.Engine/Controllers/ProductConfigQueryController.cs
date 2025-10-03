using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Controllers;

public interface IProductConfigQueryController
{
    Task<ProductConfigReply> GetProductConfigData(bool request);
    Task<LookupB8ConfigReply> GetLookupB8ConfigData(bool request);
}

public class ProductConfigQueryController : IProductConfigQueryController
{
    private readonly IMediator _mediator; 

    public ProductConfigQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<ProductConfigReply> GetProductConfigData(bool request)
    {
        return await _mediator.Send(new ProductConfigCall() { Request = request });
    }
    public async Task<LookupB8ConfigReply> GetLookupB8ConfigData(bool request)
    {
        return await _mediator.Send(new LookupB8ConfigCall() { Request = request });
    }
}
