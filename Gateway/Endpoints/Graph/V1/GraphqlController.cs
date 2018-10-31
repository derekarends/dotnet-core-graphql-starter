using System.Threading.Tasks;
using Core.Context;
using Gateway.Endpoints.Graph.V1.Model.Binding;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Endpoints.Graph.V1
{
//    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class GraphqlController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphqlController(IDocumentExecuter documentExecuter, ISchema schema)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QueryModel query)
        {
            var ctx = new ContextModel
            {
                Principal = User
            };
            
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema, 
                Query = query.Query, 
                OperationName = query.OperationName,
                Inputs = query.Variables.ToInputs(),
                UserContext = ctx,
                //ValidationRules = DocumentValidator.CoreRules().Concat(new [] { new InputValidationRule() });
            };
            
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
            if (result.Errors?.Count > 0)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}