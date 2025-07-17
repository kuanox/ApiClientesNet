using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTOs;
using UserApi.Services;

namespace UserApi.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<OkObjectResult> Update(int userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await _userService.UpdateUser(userId, updateUserDto);
        return Ok(updatedUser); // ← Ahora devuelve el DTO
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteUser(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent(); // Código 204 para eliminación exitosa
    }
}