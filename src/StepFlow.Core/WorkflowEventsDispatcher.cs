using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class WorkflowEventsDispatcher
{
    public WorkflowEvent Publish(string eventName, string? eventKey, string? eventData)
    {
        WorkflowEvent @event = new(eventName, eventKey, DateTime.UtcNow, eventData);
        string eventId = MakeEventId(@event.EventName, @event.EventKey);
        _publishedEvents[eventId] = @event;
        return @event;
    }

    public async Task<WorkflowEvent> WaitEvent(string eventName, string? eventKey)
    {
        DateTime startWaitTime = DateTime.UtcNow;
        string eventId = MakeEventId(eventName, eventKey);
        while (true)
        {
            if (_publishedEvents.TryGetValue(eventId, out WorkflowEvent @event))
            {
                if (@event.PublishDateTime > startWaitTime)
                {
                    return @event;
                }
            }

            await Task.Delay(50);
        }
    }

    private string MakeEventId(string eventName, string? eventKey)
    {
        return eventKey is null ? eventName : $"{eventName}:::{eventKey}";
    }

    private readonly ConcurrentDictionary<string, WorkflowEvent> _publishedEvents = new();
}