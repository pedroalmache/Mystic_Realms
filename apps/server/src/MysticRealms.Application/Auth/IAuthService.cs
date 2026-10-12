namespace MysticRealms.Application.Auth;

public interface IAuthService
{
    Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
