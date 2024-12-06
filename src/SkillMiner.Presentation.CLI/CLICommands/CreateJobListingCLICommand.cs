using System.CommandLine;
using System.CommandLine.Invocation;
using MediatR;
using SkillMiner.Application.CQRS.JobListingEntity.Commands;

namespace SkillMiner.Presentation.CLI.CLICommands;

public class CreateJobListingCliCommand : Command
{
    public CreateJobListingCliCommand() : base("generate-desired-skills-by-job-title", "DESCRIPTION HERE")
    {
        AddOption(new Option<string>(["--job-title", "-jt"], "Job Title" ));
    }

    public new class Handler(ISender sender) : ICommandHandler
    {
        public string JobTitle { get; set; }

        public int Invoke(InvocationContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await sender.Send(new GenerateDesiredSkillsByJobTitleCommand(JobTitle));
            
            return 0;
        }
    }
}
