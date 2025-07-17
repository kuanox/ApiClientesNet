using UserApi.DTOs;

namespace UserApi.Services;

public interface IUserService
{
    Task<UserDto> GetUserById(int id);
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto); // ← Cambia de void a UserDto
    Task<bool> DeleteUser(int id);  // Devuelve bool para confirmar eliminación
}