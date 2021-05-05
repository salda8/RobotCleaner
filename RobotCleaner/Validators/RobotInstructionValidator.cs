using FluentValidation;
using RobotCleaner.Models;

namespace RobotCleaner.Validators
{
    public class RobotInstructionValidator : AbstractValidator<RobotInstruction>
    {
        public RobotInstructionValidator()
        {
            RuleFor(ri => ri.Battery).GreaterThan(0);
            RuleFor(ri => ri.Commands).NotEmpty();
            RuleFor(ri => ri.Map).NotEmpty();
            RuleFor(ri => ri.Start).SetValidator(new LocationValidator());
        }
    }
}