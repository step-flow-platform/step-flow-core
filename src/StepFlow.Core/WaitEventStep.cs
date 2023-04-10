using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class WaitEventStep : IStep
{
    public WaitEventStep(WorkflowEventsDispatcher eventsDispatcher)
    {
        _eventsDispatcher = eventsDispatcher;
    }

    public string? EventName { get; set; }

    public string? EventData { get; private set; }

    public async Task ExecuteAsync()
    {
        if (string.IsNullOrWhiteSpace(EventName))
        {
            throw new StepFlowException("Incorrect EventName");
        }

        WorkflowEvent @event = await _eventsDispatcher.WaitEvent(EventName);
        EventData = @event.EventData;
    }

    private readonly WorkflowEventsDispatcher _eventsDispatcher;
}