using Newtonsoft.Json.Linq;

namespace Gateway.Endpoints.Graph.V1.Model.Binding
{
    public class QueryModel
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}