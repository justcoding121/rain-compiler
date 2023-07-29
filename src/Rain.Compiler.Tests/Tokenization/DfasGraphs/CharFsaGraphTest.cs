using FluentAssertions;
using Rain.Compiler.Models.Tokenization.Enums;
using Rain.Compiler.Tokenization.DfaGraphs.Interface;
using Rain.Compiler.Tokenization.DFAGraphs;

namespace Rain.Compiler.Tests.Tokenization.DfasGraphs;

[Trait("Category", "Unit")]
public class CharFsaGraphTest : IDisposable
{
    private readonly IFsa _systemUnderTest = new CharFsa();

    [Theory]
    [InlineData("a", FsaStatus.Final)]
    [InlineData("6", FsaStatus.Final)]
    [InlineData("*", FsaStatus.Final)]

    [InlineData(@"\", FsaStatus.Running)]
    [InlineData(@"\-", FsaStatus.Error)]
    [InlineData(@"\0", FsaStatus.Running)]
    [InlineData(@"\7", FsaStatus.Running)]
    [InlineData(@"\8", FsaStatus.Error)]


    [InlineData(@"\07", FsaStatus.Running)]
    [InlineData(@"\7-", FsaStatus.Error)]
    [InlineData(@"\78", FsaStatus.Error)]

    [InlineData(@"\xF", FsaStatus.Running)]
    [InlineData(@"\y", FsaStatus.Error)]
    [InlineData(@"\xFF", FsaStatus.Final)]
    [InlineData(@"\xG", FsaStatus.Error)]
    [InlineData(@"\xFG", FsaStatus.Error)]
    public void Returns_Correct_Status(string inputCode, FsaStatus expecedStatus)
    {
        for (int i = 0; i < inputCode.Length; i++)
        {
            _systemUnderTest.Read(i, inputCode[i]);
        }

        _systemUnderTest.Status.Should().Be(expecedStatus);
    }

    public void Dispose()
    {
        _systemUnderTest.Reset();
    }
}
