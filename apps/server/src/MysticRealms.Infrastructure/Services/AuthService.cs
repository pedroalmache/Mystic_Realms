using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MysticRealms.Application.Auth;
using MysticRealms.Application.Features.UserCharacters.Commands;
using MysticRealms.Domain.Identity;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager, 
    IConfiguration configuration, 
    ApplicationDbContext context, 
    IMediator mediator) : IAuthService
{
    public async Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("A user with that email already exists.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            GamerTag = request.GamerTag
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }
        
        await mediator.Send(new CreateUserCharactersCommand(user.Id), cancellationToken);

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        var validPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        var email = principal.FindFirstValue(ClaimTypes.Email)
            ?? throw new UnauthorizedAccessException("Invalid access token.");

        var user = await userManager.FindByEmailAsync(email)
            ?? throw new UnauthorizedAccessException("Invalid access token.");

        var existingToken = await context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken && r.UserId == user.Id, cancellationToken);

        if (existingToken is null || !existingToken.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        existingToken.RevokedAt = DateTime.UtcNow;

        return await GenerateTokensAsync(user, cancellationToken);
    }

    private async Task<AuthResponse> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var expiresMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "60");
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("gamerTag", user.GamerTag)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshTokenExpiry = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpiry)
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, refreshToken.Token, expiresAt, user.GamerTag ?? string.Empty);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
            ValidateLifetime = false
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwt ||
            !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }
}
