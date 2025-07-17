using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Entities;
using UserApi.Services;
using Xunit;

namespace UserApi.Tests;

public class UserServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public UserServiceTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb") // This requires the Microsoft.EntityFrameworkCore.InMemory package  
            .Options;
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        var service = new UserService(context);

        var testUser = new User
        {
            FirstName = "testfirstname",
            LastName = "testlastname",
            Email = "test@test.com",
            PasswordHash = new byte[] { 0x01 },
            PasswordSalt = new byte[] { 0x02 }
        };

        context.Users.Add(testUser);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetUserById(testUser.Id);

        // Assert
        Assert.Equals(testUser.FirstName, result.FirstName);
    }
}
