﻿namespace UserApi.DTOs;

public record UserDto(
    int Id,    
    string? FirstName,
    string? LastName,
    string Email
);
