namespace AuthService
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
    }
}
