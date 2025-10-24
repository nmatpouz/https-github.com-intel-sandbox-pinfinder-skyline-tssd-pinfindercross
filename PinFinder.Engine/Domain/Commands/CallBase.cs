namespace PinFinder.Core.Domain.Commands;
using MediatR;
public abstract record CallBase<T> : IRequest<T> where T : ReplyBase
{

}