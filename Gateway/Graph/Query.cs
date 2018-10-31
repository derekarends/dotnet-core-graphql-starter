using System.Security.Claims;
using Core.Context;
using Core.User;
using Gateway.Graph.Types.User;
using GraphQL.Types;

namespace Gateway.Graph
{
    public class Query: ObjectGraphType<object>
    {
        public Query(IUserService userService)
        {
            Name = "Query";
            UserField(userService);
             
        }

        private void UserField(IUserService userService)
        {
            FieldAsync<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the user" }
                ),
                resolve: async r =>
                {
                    var ctx = r.UserContext as ContextModel;
                    
                    return await userService.GetByIdAsync(ctx, r.GetArgument<string>("id"));
                });   
        }
    }
}