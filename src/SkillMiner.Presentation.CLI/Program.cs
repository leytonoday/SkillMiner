using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

CommandLineBuilder BuildCommandLine()
{
    var root = new RootCommand();
    root.AddCommand();
}