namespace UserApi.DTOs;

public record UpdateUserDto(
    string? Email,
    string? FirstName,
    string? LastName
);
