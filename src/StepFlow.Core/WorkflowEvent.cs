using System;

namespace StepFlow.Core;

public record WorkflowEvent(
    string EventName,
    string? EventKey,
    DateTime PublishDateTime,
    string? EventData);