using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace doe.rapido.business.DML
{
    public class User : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Confirmed { get; set; }
        public int? CodeConfirm { get; set; }
        public DateTime DtExpireCodeConfirm { get; set; }
        public DateTime DtUpdate { get; set; }
        public DateTime DtInclude { get; set; }
        public string StepOnboarding { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!String.IsNullOrEmpty(this.Email))
            {
                if (!IsValidEmail(this.Email))
                {
                    yield return new ValidationResult("Email inválido", new[] { nameof(Email) });
                }
            }

        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

