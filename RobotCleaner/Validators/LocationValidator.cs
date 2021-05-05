using FluentValidation;
using RobotCleaner.Models;

namespace RobotCleaner.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(location => location.X).GreaterThanOrEqualTo(0);
            RuleFor(location => location.Y).GreaterThanOrEqualTo(0);
            RuleFor(location => location.Facing).IsInEnum();
        }
    }
}