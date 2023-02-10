using System;
using System.Text.Json;
using StepFlow.Dsl.Model;

namespace StepFlow.Dsl
{
    public static class Deserializers
    {
        public static Func<string, WorkflowDefinitionModel?> Json
        {
            get
            {
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new WorkflowNodeJsonConverter());
                return source => JsonSerializer.Deserialize<WorkflowDefinitionModel>(source, options);
            }
        }
    }
}