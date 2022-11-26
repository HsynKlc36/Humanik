using FluentValidation;
using HumanIK.UI.Areas.Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanIK.UI.Data.FluentValidations
{
    public class AdvanceValidator : AbstractValidator<AdvanceViewModel>
    {
        public AdvanceValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithName("Miktar")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .GreaterThanOrEqualTo(1000) // Son bir yılda max 2 maaş
                .WithName("Miktar")
                .WithMessage("{PropertyName} {ComparisonValue}'den küçük olamaz")
                .LessThanOrEqualTo(x => x.RemainingAdvance)
                .WithName("Miktar")
                .WithMessage("{PropertyName} kalan miktardan fazla olamaz");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName("Açıklama")
                .WithMessage("{PropertyName} en fazla {MaxValue} karakter olabilir.");

        }
    }
}
