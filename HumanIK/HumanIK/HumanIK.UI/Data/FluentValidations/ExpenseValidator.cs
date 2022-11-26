using FluentValidation;
using HumanIK.ENTITIES.Entities;

namespace HumanIK.UI.Data.FluentValidations
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator()
        {
            RuleFor(x => x.Amount)               
               .GreaterThan(0) 
               .WithName("Miktar")
               .WithMessage("{PropertyName} {ComparisonValue}' dan küçük olamaz.");


            RuleFor(x => x.Description)
                .NotEmpty()
                .WithName("Açıklama")
                .WithMessage("{PropertyName} boş bırakılamaz.")
                .MaximumLength(250)
                .WithName("Açıklama")
                .WithMessage("{PropertyName} {MaxValue} aralığında olmak zorundadır.");

            RuleFor(x => x.CurrencyUnit)
              .IsInEnum()
              .WithName("Para Birimi")
              .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.ExpenseType)
               .IsInEnum()
               .WithName("Harcama Türü")
               .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.CreateDate)
               .GreaterThanOrEqualTo(x=>x.CreateDate)
               .WithName("Talep Tarihi")
               .WithMessage("{PropertyName} geçmiş tarih olamaz.");

            RuleFor(x => x.ExpenseFile)
                .NotEmpty()
                .WithName("Belge")
                .WithMessage("Bu alan boş geçilemez.");



        }
    }
}
