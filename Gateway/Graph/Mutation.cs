using Core.User;
using Gateway.Graph.Types;
using Gateway.Graph.Types.User;
using GraphQL.Types;

namespace Gateway.Graph
{
    public class Mutation : ObjectGraphType
    {
        public Mutation(IUserService userService)
        {
            Name = "Mutation";
    
            FieldAsync<UserType>("createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> {Name = "user"}
                ),
                resolve: async r =>
                {
                    var user = r.GetArgument<UserModel>("user");
                    return await userService.CreateAsync(user);
                });
        }
    }
}