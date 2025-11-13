# Booking App

System rezerwacji wizyt (np. gabinet / salon) z backendem w ASP.NET Core i frontendem w Next.js.

## Technologia

- **Backend**: ASP.NET Core, C#, Entity Framework Core, architektura warstwowa (Api / Application / Domain / Infrastructure)
- **Frontend**: Next.js, TypeScript, React, Tailwind CSS
- **Testy**: xUnit (testy jednostkowe domeny i testy integracyjne API)

## Struktura projektu

```text
backend/
  src/
    BookingApp.Api
    BookingApp.Application
    BookingApp.Domain
    BookingApp.Infrastructure
  tests/
    BookingApp.Domain.Tests
    BookingApp.Api.IntegrationTests

frontend/
  (aplikacja Next.js)
```
