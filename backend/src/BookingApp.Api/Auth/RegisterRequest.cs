using System;

namespace BookingApp.Api.Auth;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string Role // "Admin", "Employee", "Customer"
);