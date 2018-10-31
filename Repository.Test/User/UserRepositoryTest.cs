using System;
using AutoMapper;
using Core.User;
using FluentAssertions;
using Repository.Core;
using Repository.Test.Core;
using Repository.User;
using Xbehave;

namespace Repository.Test.User
{    
    public class UserRepositoryTests : IDisposable
    {
        private readonly MongoTestRunner _runner;
        private readonly IUserRepository _repo;

        public UserRepositoryTests()
        {
            _runner = new MongoTestRunner();
            _repo = new UserRepository(_runner.Context, new Mapper(new MapperConfiguration(a => { })));
        }
        
        [Scenario]
        public void FindByIdAsync(string id, UserModel result)
        {
            "Given I have an id".x(() =>
            {
                id = "5b9b0f9f4cc93e8138be00d2";
            });
            "And a user with that id exists".x(() =>
            {
                _runner.Context.Users.InsertOne(new UserEntity {Id = id.ToObjectId()});
            });
            "When I search using that id".x(async () =>
            {
                result = await _repo.FindByIdAsync(id);
            });
            "Then I will find a user".x(() =>
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(id);
            });
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}