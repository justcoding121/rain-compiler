using Microsoft.Extensions.DependencyInjection;
using Rain.Compiler.Interfaces;
using Rain.Compiler.Tokenization;

namespace Rain.Compiler;

internal static class DependencyRegistrations
{
    internal static void Register(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<Driver>();

        serviceCollection.AddTransient<IStep, Tokenizer>();
    }
}
