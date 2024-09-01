namespace ProductService1.Util
{
    public interface ITokenManager
    {
        int? ValidateToken(string token);
    }
}
