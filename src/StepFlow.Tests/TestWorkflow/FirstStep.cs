using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Tests.TestWorkflow;

public class FirstStep : IStep
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("-- First step --");
        return Task.CompletedTask;
    }
}