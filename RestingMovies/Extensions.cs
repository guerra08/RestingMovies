using RestingMovies.Api.Endpoints;

namespace RestingMovies.Api;

public static class Extensions
{
    public static void AddEndpointConfigurations(this IServiceCollection serviceCollection)
    {
        var endpointConfigurationInterface = typeof(IEndpointConfiguration);

        var endpointConfigurations =
            endpointConfigurationInterface.Assembly.ExportedTypes
                .Where(type =>
                    endpointConfigurationInterface.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .Select(Activator.CreateInstance).Cast<IEndpointConfiguration>()
                .ToList();

        foreach (var endpointConfiguration in endpointConfigurations)
            endpointConfiguration.AddServices(serviceCollection);

        serviceCollection.AddSingleton(endpointConfigurations as IReadOnlyCollection<IEndpointConfiguration>);
    }

    public static void UseEndpointConfigurations(this WebApplication app)
    {
        var endpointConfigurations = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointConfiguration>>();

        foreach (var endpointConfiguration in endpointConfigurations) endpointConfiguration.MapEndpoints(app);
    }
}