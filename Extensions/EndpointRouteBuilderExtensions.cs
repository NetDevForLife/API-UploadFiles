using Microsoft.AspNetCore.Routing;

namespace API_UploadFiles.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints<T>(this IEndpointRouteBuilder endpoints) where T : IEndpointRouteHandler
    => T.MapEndpoints(endpoints);
}

public interface IEndpointRouteHandler
{
    static abstract void MapEndpoints(IEndpointRouteBuilder endpoints);
}