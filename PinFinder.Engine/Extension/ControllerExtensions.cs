using Autofac;

using PinFinder.Core.Controllers;

namespace PinFinder.Core.Extension;

public static class ControllerExtensions
{
    public static void RegisterControllers(this ContainerBuilder builder)
    {
        builder.RegisterType<ItuffQueryController>().As<IItuffQueryController>().SingleInstance();
    }
}
