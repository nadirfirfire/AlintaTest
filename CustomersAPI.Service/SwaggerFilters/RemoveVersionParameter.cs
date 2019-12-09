using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.API
{
    public class RemoveVersionParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            IParameter item = operation.Parameters.Single((IParameter p) => p.Name == "version");
            operation.Parameters.Remove(item);
        }
    }
}
