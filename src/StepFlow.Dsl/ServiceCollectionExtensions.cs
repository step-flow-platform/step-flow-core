// using Microsoft.Extensions.DependencyInjection;
//
// namespace StepFlow.Dsl
// {
//     public static class ServiceCollectionExtensions
//     {
//         public static IServiceCollection AddStepFlowDsl(this IServiceCollection services,
//             WorkflowDefinitionLoaderOptions options)
//         {
//             services.AddTransient<IWorkflowDefinitionLoader>(_ => new WorkflowDefinitionLoader(options));
//             return services;
//         }
//     }
// }