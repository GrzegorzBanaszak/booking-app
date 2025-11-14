using System;

namespace BookingApp.Application.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, string fullName, IEnumerable<string> roles, out DateTime expiresAt);
}
