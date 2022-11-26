using FluentValidation;
using HumanIK.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HumanIK.REPOSITORIES.FluentValidations
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            //Resim ekleme validasyonu eklenecek
            //Mersis no ve tax no özgün olmalı

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("Şirket Adı")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(3, 50)
                .WithMessage("{PropertyName} {MinLength} - {MaxLength} karakter arası olmalıdır.")
                .Must(BeValidCompanyName)
                .WithMessage("Geçersiz şirket adı");
                //.Matches(new Regex("^[a-zA-ZwığüşöçİĞÜŞÖÇ][a-zA-Z0-9wığüşöçİĞÜŞÖÇ]+$"))
                //.WithMessage("Lütfen en az bir tane harf giriniz");

            RuleFor(x => x.TaxAdministration)
                .NotEmpty()
                .WithName("Vergi Dairesi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(5, 50)
                .WithMessage("{PropertyName} {MinLength} - {MaxLength} karakter arası olmalıdır.")
                .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ ]+$"))
                .WithMessage("Lütfen yalnızca harf girin.");

            RuleFor(x => x.TaxNo)
                .NotEmpty()
                .WithName("Vergi Numarası")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(10)
                .WithMessage("{PropertyName} {MinLength} karakter olmalıdır.")
                .Matches(new Regex(@"^[0-9]*$"))
                .WithMessage("Lütfen yalnızca sayı girin.");

            RuleFor(x => x.MersisNo)
                .NotEmpty()
                .WithName("Mersis No")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(16)
                .WithMessage("{PropertyName} {MinLength} karakter olmalıdır.")
                .Matches(new Regex(@"^[0-9]*$"))
                .WithMessage("Lütfen yalnızca sayı girin.");

            RuleFor(x => x.Founded)
                .NotEmpty()
                .WithName("Kuruluş Tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .GreaterThan(new DateTime(1800, 1, 1))
                .WithMessage("{PropertyName} {ComparisonValue} tarihinden önce olamaz.");

            RuleFor(x => x.DealStartDate)
                .NotEmpty()
                .WithName("Anlaşma Başlangıç Tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .GreaterThan(new DateTime(2020, 1, 1))
                .WithMessage("{PropertyName} {ComparisonValue} tarihinden önce olamaz.")
                .GreaterThan(x => x.Founded).WithMessage("Anlaşma Başlangıç Tarihi, Kuruluş Tarihinden sonra olmak zorundadır.");

            RuleFor(x => x.DealEndDate)
                .NotEmpty()
                .WithName("Anlaşma Bitiş Tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .GreaterThanOrEqualTo(x => x.DealStartDate)
                .WithMessage("{PropertyName} başlangıç tarihinden sonra olmalı.");

            RuleFor(x => x.Address)
                .MaximumLength(250)
                .WithName("Adres")
                .WithMessage("{PropertyName} en fazla {MaxLength} karakter olabilir.");

            RuleFor(x => x.NumberOfEmployees)
                .NotEmpty()
                .WithName("Çalışan Sayısı")
                .WithMessage("{PropertyName} alanı boş bırakılamaz")
                .InclusiveBetween(1, 100000)
                .WithMessage("{PropertyName} değer aralığı 1 ile 100000 arasında girilmelidir");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithName("Telefon Numarası")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Matches(new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"))
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz.");
            

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithName("E-Posta")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .EmailAddress()
                .WithMessage("Lütfen geçerli bir {PropertyName} giriniz.");

            RuleFor(x => x.Sector)
                .IsInEnum()
                .WithName("Sektör")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithName("Şirket Türü")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        }

        private bool BeValidCompanyName(string companyName)
        {
            if(!companyName.Any(x=> char.IsLetter(x)))  // En az bir harf olma şartı
                return false;

            if (companyName.Any(x => char.IsPunctuation(x)))
                return false;

            return true;
        }

    }
}
