namespace ProductService.Util
{
    public interface ITokenManager
    {
        int? ValidateToken(string token);
    }
}
