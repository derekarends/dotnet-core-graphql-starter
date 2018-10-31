using Gateway.Graph;
using Gateway.Graph.Types.User;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Deps
{
    public static class GraphQLRegistration
    {
        public static void AddGraphQL(this IServiceCollection services)
        {   
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<ISchema, DocFlowSchema>();
            services.AddSingleton<Query>();
            services.AddSingleton<Mutation>();
            services.AddSingleton<UserType>();
            services.AddSingleton<UserInputType>();   
        }
    }
}