using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Entities;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        return new UserDto(
            user.Id,            
            user.FirstName,
            user.LastName,
            user.Email
        );
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(u => new UserDto(
            u.Id,            
            u.FirstName,
            u.LastName,
            u.Email
        ));
    }

    public async Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        // ... lógica de actualización ...
        await _context.SaveChangesAsync();
        return MapToUserDto(user); // ← Devuelve el DTO actualizado
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false; // Usuario no encontrado
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true; // Eliminación exitosa
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto(
            user.Id,            
            user.FirstName,
            user.LastName,
            user.Email
        );
    }
}