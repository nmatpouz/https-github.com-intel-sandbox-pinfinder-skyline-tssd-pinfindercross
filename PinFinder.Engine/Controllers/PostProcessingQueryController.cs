using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Controllers;

public interface IPostProcessingQueryController
{
    Task<PostProcessingReply> GetPostProcessingData(string config,string lotID, string opCdode, string summaryName);
}

public class PostProcessingQueryController : IPostProcessingQueryController
{
    private readonly IMediator _mediator;

    public PostProcessingQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<PostProcessingReply> GetPostProcessingData(string config, string lotID, string opCdode, string summaryName)
    {
        return await _mediator.Send(new PostProcessingCall() { Config = config, LotId = lotID, OperCode = opCdode, SummaryName = summaryName });
    }
}
