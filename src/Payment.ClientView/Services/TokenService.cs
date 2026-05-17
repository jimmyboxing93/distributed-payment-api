using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Payment.ClientView.Services
{
	public class TokenService : ITokenService
	{
		private readonly SymmetricSecurityKey _key;
		private readonly IConfiguration _config;

		public TokenService(IConfiguration config) 
		{
			_config = config;
			var secret = Environment.GetEnvironmentVariable("JWT_KEY") ?? config["Jwt:Key"];
			if (string.IsNullOrEmpty(secret) || secret.Length < 32) 
			{
				throw new Exception("JWT Key is missing or too short! Check your .env file.");
			}
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
		}

		public string CreateToken(IdentityUser user) 
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email)
			};

			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

			var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? _config["Jwt:Issuer"];
			var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? _config["Jwt:Audience"];

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = creds,
				Issuer = issuer,
				Audience = audience
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

	}
}
