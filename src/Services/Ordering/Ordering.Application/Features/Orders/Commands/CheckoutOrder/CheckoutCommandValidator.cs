using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(o => o.UserName).NotEmpty().WithMessage("{UserName} is required").NotNull().MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters");
            RuleFor(o => o.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required").NotNull();
            RuleFor(o => o.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required").NotNull().GreaterThan(0).WithMessage("{TotalPrice} must be greater than zero.");
        }
    }
}
