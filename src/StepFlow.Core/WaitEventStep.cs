using System;
using System.Threading.Tasks;
using StepFlow.Contracts;

namespace StepFlow.Core
{
    internal class WaitEventStep : IStep
    {
        public string? EventName { get; set; }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
