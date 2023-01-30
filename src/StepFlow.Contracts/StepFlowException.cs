using System;

namespace StepFlow.Contracts
{
    public class StepFlowException : Exception
    {
        public StepFlowException(string message)
            : base(message)
        {
        }

        public StepFlowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}