namespace BookingApp.Api.Auth;

public sealed record AuthResponse(string Token, DateTime Expiration);