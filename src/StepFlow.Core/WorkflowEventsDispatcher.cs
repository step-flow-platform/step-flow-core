using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StepFlow.Core;

internal class WorkflowEventsDispatcher
{
    public void Publish(string eventName, string? eventData)
    {
        WorkflowEvent @event = new(eventName, DateTime.UtcNow, eventData);
        _publishedEvents[@event.EventName] = @event;
    }

    public async Task<WorkflowEvent> WaitEvent(string eventName)
    {
        DateTime startWaitTime = DateTime.UtcNow;
        while (true)
        {
            if (_publishedEvents.TryGetValue(eventName, out WorkflowEvent @event))
            {
                if (@event.PublishDateTime > startWaitTime)
                {
                    return @event;
                }
            }

            await Task.Delay(50);
        }
    }

    private readonly ConcurrentDictionary<string, WorkflowEvent> _publishedEvents = new();
}