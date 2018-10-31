using Core.Context;
using Core.Email;
using Core.Token;
using Core.User;
using FluentAssertions;
using Moq;
using Service.User;
using Xbehave;

namespace Service.Test.User
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;

        public UserServiceTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userService = new UserService(_userRepoMock.Object, _emailServiceMock.Object, _tokenServiceMock.Object);
        }
        
        [Scenario]
        public void AuthenticateAsync(string email, string password, UserModel result)
        {
            "Given a valid email and password".x(() =>
            {
                email = "test@test.com";
                password = "asdf";
            });
            "When I try to authenticate".x(async () =>
            {
                var repoResult = new UserModel
                {
                    Email = email,
                    Password = "$2a$11$uwZtVmH4cQOXXNsAzL.yzeAfadNz3i5EGHRNzo2UZnJ2KuMMA4laO"
                };
                _userRepoMock.Setup(s => s.FindByEmailAsync(email)).ReturnsAsync(repoResult);
                result = await _userService.AuthenticateAsync(email, password);
            });
            "Then I will be authenticated".x(() =>
            {
                result.Should().NotBeNull();
                result.Email.Should().Be(email);
            });
        }
    }
}