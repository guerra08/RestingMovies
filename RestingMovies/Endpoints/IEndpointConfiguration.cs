namespace RestingMovies.Api.Endpoints;

public interface IEndpointConfiguration
{
    void AddServices(IServiceCollection services);
    void MapEndpoints(WebApplication app);
}