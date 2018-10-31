using System.Text;
using System.Threading;
using Core.User;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using Repository.User;
using Xbehave;

namespace Repository.Test.User
{
    public class UserCacheTest
    {
        private readonly UserCache _userCache;
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IDistributedCache> _cacheMock;

        public UserCacheTest()
        {
            _repoMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<IDistributedCache>();
            _userCache = new UserCache(_repoMock.Object, _cacheMock.Object);
        }

        [Scenario]
        public void FindByIdAsync(string id, UserModel result)
        {
            "Given I have an id".x(() =>
            {
                id = "5b9b0f9f4cc93e8138be00d3";
            });
            "And a user with that id exists in cache".x(() =>
            {
                var serializedUser = JsonConvert.SerializeObject(new UserModel { Id = id });
                var byteUser = Encoding.UTF8.GetBytes(serializedUser);
                _cacheMock.Setup(s => s.GetAsync(id, CancellationToken.None)).ReturnsAsync(byteUser);
            });
            "When I search using that id".x(async () =>
            {
                result = await _userCache.FindByIdAsync(id);
            });
            "Then I will find the user from cache".x(() =>
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(id);
                _cacheMock.Verify(v => v.GetAsync(id, CancellationToken.None), Times.Once);
            });
            "And not from the repo".x(() =>
            {
                _repoMock.Verify(v => v.FindByIdAsync(It.IsAny<string>()), Times.Never);
            });
        }
    }
}