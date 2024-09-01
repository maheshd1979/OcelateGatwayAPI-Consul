using AuthService.Util;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthService.EndPoints
{
    public class AuthEndPoint : IEndPoint
    {
        public void MapEndpoint(WebApplication app)
        {
            var group = app.MapGroup("api/auth");

            group.MapPost("/token", GetToken)
                .Accepts<LoginRequestDTO>("application/json")
                .Produces<APIResponse>(200).Produces(400)
                // .AddEndpointFilter<ValidationFilter<LoginRequestDTO>>()
                .WithName("GetToken")
                .WithOpenApi();


        }

        private async static Task<IResult> GetToken(ITokenManager tokenManager,
          [FromBody] LoginRequestDTO model)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            //var UserDTO = await _userService.GetUser(model.UserName, model.Password);

            var UserDTO = new UserDTO
            {
                ID = 1,
                UserName = model.UserName,
                Name = model.Password
            };

            //var loginResponse = await _authRepo.Login(model);
            if (UserDTO == null)
            {
                //    response.ErrorMessages.Add("Username or password is incorrect");
                //    return Results.BadRequest(response);
                var failureResponse = new APIResponse
                {
                    Result = $"Username or password is incorrect.",
                    IsSuccess = false,
                    // StatusCode = HttpStatusCode.Unauthorized
                };
                return Results.Unauthorized();
                // return Results.Unauthorized(failureResponse);
            }

            var loginResponse = await tokenManager.GenerateAccessToken(UserDTO);

            response.Result = loginResponse;
            response.IsSuccess = true;
            //response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);

        }

    }
}
