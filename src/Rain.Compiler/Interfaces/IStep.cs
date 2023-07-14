using Rain.Compiler.Models;

namespace Rain.Compiler.Interfaces;

public interface IStep
{
    void Execute(State state);
}
