using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Queries.Get;

namespace PinFinder.Core.Extension;

public static class MediatRExtensions
{
    public static IServiceCollection RegisterRequestHandlers(
        this IServiceCollection services)
    {
        return services
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(GetUnitQueryHandler).Assembly))
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(GetHeaderQueryHandler).Assembly));
    }
    public static void PopulateMediaTCollection(this ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(HeaderCall).Assembly);
            config.RegisterServicesFromAssembly(typeof(UnitCall).Assembly);
        });

        builder.Populate(services);
    }
}