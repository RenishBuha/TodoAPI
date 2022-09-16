using JWT.Algorithms;
using JWT.Builder;
using System.Text;
using TodoAPI.Auth;
using TodoAPI.Models;

namespace Bethubs.Auth
{
    public static class AuthUtil
    {
        public static ProcessResult<TokenData> DecodeJwtToken(string token, string secretKey,bool verify = true)
        {
            try
            {
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

                var builder = new JwtBuilder();
                var algorithm = new HMACSHA256Algorithm();

                var jwt = builder.WithAlgorithm(algorithm)
                            .WithSecret(secretKeyBytes)
                            .WithVerifySignature(verify)
                            .Decode<Dictionary<string, object>>(token);

                return ProcessResult<TokenData>.Success(new TokenData
                {
                    UserId = jwt["Sid"].ToString(),
                    Username = jwt["Name"].ToString(),
                    Role = jwt["Role"].ToString(),
                    Email = jwt["Email"].ToString(),
                    Expiration = Convert.ToDateTime(jwt["Expiration"]),
                    IpAddress = jwt["ip"].ToString()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"INVALID: {token}. ERROR: {e.Message}");
                return ProcessResult<TokenData>.Fail(new Error("Token", e.Message));
            }
        }

        public static string EncodeToken(TokenData payload, string secretKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var builder = new JwtBuilder();
            var algorithm = new HMACSHA256Algorithm();

            var token = builder.WithAlgorithm(algorithm)
                                .WithSecret(secretKeyBytes)
                                .AddClaim("Sid", payload.UserId)
                                .AddClaim("Name", payload.Username)
                                .AddClaim("Role", payload.Role)
                                .AddClaim("Email", payload.Email)
                                .AddClaim("Expiration", payload.Expiration)
                                .AddClaim("ip", payload.IpAddress)
                                .Encode();
            return token;
        }
    }
}
