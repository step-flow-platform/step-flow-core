using System;

namespace StepFlow.Contracts;

public record WorkflowEvent(
    string EventName,
    string? EventKey,
    DateTime PublishDateTime,
    string? EventData);