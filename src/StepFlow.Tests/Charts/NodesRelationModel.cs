namespace StepFlow.Tests.Charts;

public record NodesRelationModel(
    NodeModel From,
    NodeModel To,
    string? Title);