using System.Collections.Generic;

namespace Core.User
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<string> Roles { get; set; }
    }
}