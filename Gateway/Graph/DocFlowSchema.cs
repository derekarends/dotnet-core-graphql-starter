using GraphQL;
using GraphQL.Types;

namespace Gateway.Graph
{
    public class DocFlowSchema : Schema
    {
        public DocFlowSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<Query>();
            Mutation = resolver.Resolve<Mutation>();
        }
    }
}