using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Data;
using System.Text;
using MovieNightScheduler.Models;
using MovieNightScheduler.Helpers;
using Dapper;

namespace MovieNightScheduler.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string token);
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
    public class JwtUtils : IJwtUtils
    {
        private AppDb Db;
        private readonly AppSettings _appsettings;

        public JwtUtils(AppDb db, IOptions<AppSettings> appSettings)
        {
            Db = db;
            _appsettings = appSettings.Value;
        }
        public string GenerateJwtToken(User user)
        {
            //generate token with a valid time of 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public int? ValidateJwtToken(string token)
        {
            if (token == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //set clocksew to 0 so tokens expire on time
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                //return user id from token if validation is successful
                return userId;
                
            }
            catch { return null; }
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = getUniqueToken(),
                //token is valid for 5 days
                Expires = DateTime.UtcNow.AddDays(5),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            return refreshToken;

            string getUniqueToken()
            {
                // token is a cryptographically strong random sequence of values
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var parameters = new DynamicParameters();
                parameters.Add(token, token, DbType.String);
                // ensure token is unique by checking against db
                var query = "select id, token from users JOIN tokens on users.Id = tokens.userId where token = @token";
                var tokenIsUnique = Db.Connection.Query(query, parameters);
                    //!_context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

                if (!tokenIsUnique.First().id)
                    return getUniqueToken();

                return token;
            }
        }
    }
}
