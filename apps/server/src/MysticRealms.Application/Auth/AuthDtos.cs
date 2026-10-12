namespace MysticRealms.Application.Auth;

public record SignUpRequest(string Email, string Password, string GamerTag);
public record LoginRequest(string Email, string Password);
public record RefreshTokenRequest(string AccessToken, string RefreshToken);
public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, string GamerTag);
