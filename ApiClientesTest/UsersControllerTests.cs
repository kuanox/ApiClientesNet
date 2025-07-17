using Microsoft.AspNetCore.Mvc;
using Moq;
using UserApi.Controllers;
using UserApi.DTOs;
using UserApi.Services;

namespace ApiClientesTest
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IUserService>? _mockUserService;
        private UsersController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService!.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto(1, "user1", "user1@example.com", "First1", "Last1", DateTime.UtcNow),
                new UserDto(2, "user2", "user2@example.com", "First2", "Last2", DateTime.UtcNow)
            };

            _mockUserService.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUsers = okResult.Value as IEnumerable<UserDto>;
            Assert.IsNotNull(returnedUsers);
        }

        [TestMethod]
        public async Task GetById_ReturnsOkResult_WithUser()
        {
            // Arrange
            int userId = 1;
            var user = new UserDto(userId, "user1", "user1@example.com", "First1", "Last1", DateTime.UtcNow);

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUser = okResult.Value as UserDto;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(userId, returnedUser.Id);
        }

        [TestMethod]
        public async Task Update_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDto("newemail@example.com", "NewFirst", "NewLast");
            var updatedUser = new UserDto(userId, "user1", "newemail@example.com", "NewFirst", "NewLast", DateTime.UtcNow);

            _mockUserService.Setup(s => s.UpdateUser(userId, updateUserDto)).ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.Update(userId, updateUserDto);

            // Assert
            var okResult = result.Value as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUser = okResult.Value as UserDto;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual("newemail@example.com", returnedUser.Email);
            Assert.AreEqual("NewFirst", returnedUser.FirstName);
        }

        [TestMethod]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(s => s.DeleteUser(userId)).ReturnsAsync(true); // Fix for CS1503: Use ReturnsAsync instead of Returns

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}