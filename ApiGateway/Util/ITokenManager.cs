namespace ApiGateway.Util
{
    public interface ITokenManager
    {
        int? ValidateToken(string token);
    }
}
