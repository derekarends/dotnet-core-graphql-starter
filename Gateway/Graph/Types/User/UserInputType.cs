using Core.User;
using GraphQL.Types;

namespace Gateway.Graph.Types.User
{
    public class UserInputType: InputObjectGraphType<UserModel>
    {
        public UserInputType()
        {
            Name = "UserInput";
            Field(f => f.Name).Description("Name of user");
            Field(f => f.Email).Description("Email of user");
            Field(f => f.Password).Description("Password of user");
        }
    }
}