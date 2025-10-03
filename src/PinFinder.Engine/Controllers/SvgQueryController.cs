using MediatR;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Domain.Commands.Replies;

namespace PinFinder.Core.Controllers;

public interface ISvgQueryController
{
    Task<SvgReply> GetSvgData(string tiuDesignID,string mUDesignID, string pkg, string prgnm, string tiuID);
}

public class SvgQueryController : ISvgQueryController
{
    private readonly IMediator _mediator;

    public SvgQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<SvgReply> GetSvgData(string tiuDesignID, string mUDesignID, string pkg, string prgnm, string tiuID)
    {
        return await _mediator.Send(new SvgCall() { TiuDesignID = tiuDesignID, MUDesignID = mUDesignID, Pkg = pkg, Prgnm = prgnm, TiuID = tiuID});
    }
}
