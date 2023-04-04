using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Cvijecara_Sanja_Tica_IT80_2019.AuthHelpers
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _key;

        public AuthRepository(string key)
        {
            this._key = key;
        }
        public AuthToken Authenticate(string korisnickoIme, string lozinka, string tipKorisnika)
        {
            var tknHandler = new JwtSecurityTokenHandler();
            var tknKey = Encoding.ASCII.GetBytes(_key);
            var tknDscrptr = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,korisnickoIme),
                    new Claim(ClaimTypes.Role, tipKorisnika)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tknKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tknHandler.CreateToken(tknDscrptr);
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role,tipKorisnika)
                };
            }
            AuthToken tokenResp = new AuthToken
            {
                Token = tknHandler.WriteToken(token),
                Expires = String.Format("{0:dd-MM-yyyy hh:mm:ss}", (DateTime)tknDscrptr.Expires)
            };

            return tokenResp;
        }
    }
}
