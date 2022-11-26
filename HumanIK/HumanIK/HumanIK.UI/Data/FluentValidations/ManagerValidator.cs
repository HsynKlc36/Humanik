using FluentValidation;
using HumanIK.ENTITIES.Entities;
using HumanIK.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HumanIK.REPOSITORIES.FluentValidations
{
    public class ManagerValidator : AbstractValidator<ManagerViewModel>
    {
        public ManagerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("Adı")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(2, 15)
                .WithName("Adı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.SecondName)
                .Length(2, 15)
                .WithName("İkinci adı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithName("Soyadı")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(2, 15)
                .WithName("Soyadı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.SecondLastName)
                .Length(2, 15)
                .WithName("İkinci soyadı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithName("Adres")
                .WithMessage("{PropertyName} boş bırakılamaz.")
                .Length(3, 250)
                .WithName("Adres")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithName("Doğum Tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Custom((date, context) =>
                {
                    DateTime from = new DateTime(1940, 1, 1);
                    DateTime to = DateTime.Now.AddYears(-18);
                    if (date < from)
                        context.AddFailure($"Doğum Tarihi {from.Date} tarihinden sonra olmalı");
                    else if (date > to)
                        context.AddFailure($"Doğum Tarihi {to.Day}.{to.Month}.{to.Year} tarihinden önce olmalı");
                });

            RuleFor(x => x.PersonalPhone)
                .Matches(new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"))
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.CitizenId)
                .NotEmpty()
                .WithName("T.C. Kimlik Numarası")
                .WithMessage("{PropertyName} boş bırakılamaz.")
                .Length(11)
                .WithName("T.C. Kimlik Numarası")
                .WithMessage("Hatalı bir {PropertyName} girdiniz. Lütfen tekrar giriniz.")
                .Matches(new Regex(@"^[1-9]{1}[0-9]{9}[02468]{1}$"))
                .WithMessage("Hatalı bir {PropertyName} girdiniz. Lütfen tekrar giriniz.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithName("İşe başlangıç tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .GreaterThan(x => x.Company.Founded.Date)
                .WithMessage("{PropertyName} şirketin kuruluş tarihi olan {ComparisonValue} tarihinden sonra olmalı")
                .LessThan(DateTime.Now.Date.AddDays(7))
                .WithMessage("{PropertyName} en fazla 7 gün sonra olabilir.");

            RuleFor(x => x.Job)
                .IsInEnum()
                .WithName("Meslek")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.Department)
                .NotEmpty()
                .WithName("Departman")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithName("Adres")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(3, 250)
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} karakter arasında olmalıdır.");

        }
    }
}
