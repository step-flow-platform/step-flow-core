using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StepFlow.Dsl.Model;

namespace StepFlow.Dsl;

internal class WorkflowNodeJsonConverter : JsonConverter<WorkflowNodeModel>
{
    public override WorkflowNodeModel Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        Utf8JsonReader nodeReader = reader;
        if (nodeReader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        nodeReader.Read();
        if (nodeReader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        string? propertyName = nodeReader.GetString();
        if (propertyName != "name")
        {
            throw new JsonException();
        }

        nodeReader.Read();
        if (nodeReader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string? typeName = nodeReader.GetString();
        return typeName switch
        {
            null => throw new JsonException(),
            "+If" => JsonSerializer.Deserialize<WorkflowBranchModel>(ref reader, options)!,
            _ => JsonSerializer.Deserialize<WorkflowStepModel>(ref reader, options)!
        };
    }

    public override void Write(Utf8JsonWriter writer, WorkflowNodeModel value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}