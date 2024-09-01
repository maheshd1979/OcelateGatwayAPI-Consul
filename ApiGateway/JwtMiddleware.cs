using ApiGateway.Util;

namespace ApiGateway
{
    public class JwtMiddleware
    {
        // RequestDelegate to invoke the next middleware in the pipeline.
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware asynchronously to handle JWT authentication.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="userService">The service for managing user-related operations.</param>
        /// <param name="jwtUtils">The utility for working with JWT tokens.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context, ITokenManager jwtUtils)
        {
            //if (!context.Request.Path.StartsWithSegments("/api/auth"))
            //{
            // Retrieve the JWT token from the Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validate the JWT token and extract the user ID
            var userId = jwtUtils.ValidateToken(token);

            // If the token is valid, attach the user ID to the context
            if (userId != null)
            {
                // Attach user ID to context on successful JWT validation
                context.Items["User"] = userId; // Optionally, retrieve user details using userService.GetById(userId.Value)
            }


            // Continue to the next middleware in the pipeline
            await _next(context);
            // }
        }


    }
}
