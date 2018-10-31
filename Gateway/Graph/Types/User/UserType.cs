using Core.User;
using GraphQL.Types;

namespace Gateway.Graph.Types.User
{
    public class UserType : ObjectGraphType<UserModel>
    {
        public UserType()
        {
            Name = "User";
            Field(f => f.Id).Description("Id of user");
            Field(f => f.Name).Description("Name of user");
            Field(f => f.Email).Description("Email of user");
        }
    }
}