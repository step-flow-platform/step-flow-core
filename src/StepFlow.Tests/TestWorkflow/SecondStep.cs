using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class SecondStep : IStep
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("-- Second step --");
        return Task.CompletedTask;
    }
}