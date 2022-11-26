using FluentValidation;
using HumanIK.UI.Areas.Employee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Data.FluentValidations
{
    public class PermissionValidator: AbstractValidator<PermissionViewModel>
    {
        public PermissionValidator()
        {
            RuleFor(x => x.PermissionType)
              .IsInEnum()
              .WithName("İzin Türü")
              .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.NumberOfDays)
             .NotEmpty()
             .WithName("İzin Gün Sayısı")
             .WithMessage("{PropertyName} alanı boş bırakılamaz.")
             .GreaterThan(0)
             .WithName("İzin Gün Sayısı")
             .WithMessage("{PropertyName} {ComparisonValue} 'dan küçük olamaz.");
             

            RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithName("İzin Başlangıç Tarihi")
            .WithMessage("{PropertyName} alanı boş bırakılamaz.")
              .Custom((date, context) =>
              {
                  DateTime from = DateTime.Now;
                  DateTime to = DateTime.Now.AddDays(90);
                  if (date < from)
                      context.AddFailure($"İzin başlangıç Tarihi {from.Date} tarihinden sonra olmalı");
                  else if (date > to)
                      context.AddFailure($"İzin başlangıç Tarihi {to.Day}.{to.Month}.{to.Year} tarihinden önce olmalı");
              });

            RuleFor(x => x.EndDate)
          .NotEmpty()
          .WithName("İzin Bitiş Tarihi")
          .WithMessage("{PropertyName} alanı boş bırakılamaz.")
          .GreaterThan(x => x.StartDate)
          .WithName("İzin Bitiş Tarihi")
          .WithMessage("{PropertyName} {ComparisonValue} önce olamaz");


            RuleFor(x => x.CreateDate)
                   .GreaterThanOrEqualTo(x => x.CreateDate)
                   .WithName("Talep Tarihi")
                   .WithMessage("{PropertyName} geçmiş tarih olamaz.");

        }
    }
}
