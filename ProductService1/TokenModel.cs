namespace ProductService1
{
    public class TokenModel
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan AccessExpiration { get; set; }
    }
}
