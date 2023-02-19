using System;
using System.Text.Json;
using StepFlow.Dsl.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

        public static Func<string, WorkflowDefinitionModel?> Yaml
        {
            get
            {
                IDeserializer deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();
                return source => deserializer.Deserialize<WorkflowDefinitionModel>(source);
            }
        }
    }
}