using System.Threading.Tasks;

namespace StepFlow.Contracts
{
    public interface IStep
    {
        Task ExecuteAsync();
    }
}