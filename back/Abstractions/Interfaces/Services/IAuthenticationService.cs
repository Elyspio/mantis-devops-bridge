using System.IdentityModel.Tokens.Jwt;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

public interface IAuthenticationService
{
	bool ValidateJwt(string? token, out JwtSecurityToken? validatedToken);
}