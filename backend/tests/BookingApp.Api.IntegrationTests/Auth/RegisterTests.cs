using System;
using System.Net;
using System.Net.Http.Json;
using BookingApp.Api.Auth;
using FluentAssertions;

namespace BookingApp.Api.IntegrationTests.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task Register_ShouldReturnToken_WhenDataIsValid()
    {
        // Arrange
        var request = new RegisterRequest(
            Email: $"test_{Guid.NewGuid():N}@example.com", // unikalny email
            Password: "Test123!",
            FullName: "Test User",
            Role: "Customer"
        );
        // Act

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        // Assert
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        auth.Should().NotBeNull();
        auth.Token.Should().NotBeNullOrEmpty();
    }
}
