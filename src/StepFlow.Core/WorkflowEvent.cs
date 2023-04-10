using System;

namespace StepFlow.Core;

public record WorkflowEvent(
    string EventName,
    DateTime PublishDateTime,
    string? EventData);