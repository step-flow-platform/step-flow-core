using System;
using System.Threading.Tasks;

namespace StepFlow.Core;

internal interface IWorkflowStep
{
    public Task Execute(IServiceProvider serviceProvider, object data);
}