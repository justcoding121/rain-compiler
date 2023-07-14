using Rain.Compiler.Interfaces;
using Rain.Compiler.Models;

namespace Rain.Compiler;

internal class Driver
{
    private readonly List<IStep> _steps;
    internal Driver(IEnumerable<IStep> steps) {
        _steps = steps.ToList();
    }

    internal void Drive(string sourceCode)
    {
        var state = new State()
        {
            Source = sourceCode
        };

        _steps.ForEach(step => { step.Execute(state); });
    }
}
