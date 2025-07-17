using ApiClientesNet.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Entities;

namespace UserApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(AuthResponseDto authResponseDto)
        {
            // Validar si el Email ya está en uso
            if (await _context.Users.AnyAsync(u => u.Email == authResponseDto.Email))
                throw new Exception("El email ya está en uso.");

            // Crear el hash y salt de la contraseña
            PasswordHasher.CreatePasswordHash(authResponseDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Crear el usuario con la información proporcionada
            var user = new User
            {
                FirstName = authResponseDto.FirstName,
                LastName = authResponseDto.LastName,
                Email = authResponseDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = authResponseDto.Password // Aunque usualmente no se retorna la contraseña
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            // Buscar el usuario por email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                throw new Exception("Email o contraseña inválidos.");

            // Verificar la contraseña
            bool isValid = PasswordHasher.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValid)
                throw new Exception("Email o contraseña inválidos.");

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = loginDto.Password
            };
        }

        // Método auxiliar para generar el token JWT
        private string GenerateJwtToken(User user)
        {
            // Crear las afirmaciones (claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            // Leer la clave secreta del archivo de configuración
            var secret = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("JWT secret token not configured.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            // Crear las credenciales
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Crear el token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            // Crear el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
