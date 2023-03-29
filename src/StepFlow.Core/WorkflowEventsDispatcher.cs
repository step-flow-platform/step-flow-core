using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StepFlow.Core;

internal class WorkflowEventsDispatcher
{
    public void Publish(string eventName)
    {
        _publishedEvents[eventName] = DateTime.UtcNow;
    }

    public async Task WaitEvent(string eventName)
    {
        DateTime startWaitTime = DateTime.UtcNow;
        while (true)
        {
            if (_publishedEvents.TryGetValue(eventName, out DateTime publishTime))
            {
                if (publishTime > startWaitTime)
                {
                    return;
                }
            }

            await Task.Delay(50);
        }
    }

    private readonly ConcurrentDictionary<string, DateTime> _publishedEvents = new();
}