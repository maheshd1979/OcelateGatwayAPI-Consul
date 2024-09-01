namespace AuthService.Util
{
    public interface ITokenManager
    {
        Task<LoginResponseDTO> GenerateAccessToken(UserDTO user);

        int? ValidateToken(string token);
    }
}
