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
                return source => JsonSerializer.Deserialize<WorkflowDefinitionModel>(
                    source, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }
    }
}