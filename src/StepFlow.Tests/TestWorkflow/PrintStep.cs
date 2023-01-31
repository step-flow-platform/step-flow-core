using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class PrintStep : IStep
{
    public string Line { get; set; } = default!;

    public Task ExecuteAsync()
    {
        Console.WriteLine(Line);
        return Task.CompletedTask;
    }
}