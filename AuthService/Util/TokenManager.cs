using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Util
{
    public class TokenManager(IOptions<TokenModel> tokenModel) : ITokenManager
    {

        private readonly TokenModel _tokenModel = tokenModel.Value;

        public async Task<LoginResponseDTO> GenerateAccessToken(UserDTO user)
        {

            var claims = GetUserClaims(user);
            var key = Encoding.ASCII.GetBytes(_tokenModel.SecretKey);

            //  var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenModel.SecretKey)), SecurityAlgorithms.HmacSha256);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var now = DateTime.UtcNow;

            _ = claims?.Append(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));


            var jwt = new JwtSecurityToken(
                issuer: _tokenModel.Issuer,
                audience: _tokenModel.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_tokenModel.AccessExpiration),
                signingCredentials: credentials);


            var response = new LoginResponseDTO
            {
                User = user,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
            };

            return response;
        }

        private static List<Claim> GetUserClaims(UserDTO user)
        {
            var claims = new List<Claim> {
                               //new Claim (JwtRegisteredClaimNames.Sub, EncryptDecrypt.Encrypt(user.UserId.ToString())),
                               new Claim (JwtRegisteredClaimNames.Jti, user.ID.ToString()),
                               new Claim(JwtRegisteredClaimNames.Name, user.UserName)
                              //  new Claim(JwtRegisteredClaimNames.
                               //new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                               //new Claim(JwtRegisteredClaimNames.Email, user.UserEmail)
                             //  new Claim(ClaimTypes.Name,user.UserName),
                             // new Claim(ClaimTypes.i,roles.FirstOrDefault()),
                           };

            //foreach (var roleId in user.RoleIds)
            //{
            //    claims.Add(new Claim("role", Convert.ToString(roleId)));
            //}

            return claims;
        }


        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenModel.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "jti").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

    }
}
