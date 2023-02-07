using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core;

internal class IfStep : IStep
{
    public bool Condition { get; set; } = default!;

    public Task ExecuteAsync()
    {
        Console.WriteLine(Condition);

        return Task.CompletedTask;
    }
}