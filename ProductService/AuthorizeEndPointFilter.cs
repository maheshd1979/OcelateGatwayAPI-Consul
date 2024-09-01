namespace ProductService
{
    public class AuthorizeEndPointFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (context.HttpContext.Items["User"] == null)
            {
                // Handle the case where user information is not available (e.g., user is not authenticated)
                return Results.Unauthorized();
                //return ApiResponseBuilder.CreateUnauthorizedResponse("User information not available. Authentication may be required.");
            }

            var result = await next(context);

            // After the endpoint is called

            return result;
        }
    }
}
