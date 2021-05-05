using System.IO;
using FluentValidation;
using RobotCleaner.Models;

namespace RobotCleaner.Validators
{
    public class InputArgumentsValidator : AbstractValidator<InputArguments>
    {
        public InputArgumentsValidator()
        {
            RuleFor(args => args.RobotInstructionsFileLocation).NotEmpty()
                .Must(File.Exists).WithMessage("Please ensure that the file {PropertyValue} exists");
            RuleFor(args => args.RobotResultFileLocation).NotEmpty();
        }
    }
}