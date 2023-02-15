namespace StepFlow.Tests.Charts;

public record NodesRelationModel(
    string FromId,
    string ToId,
    string? Title)
{
    public string ToId { get; set; } = ToId;
}