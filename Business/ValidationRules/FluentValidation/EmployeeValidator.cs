using Business.Constants;
using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class EmployeeValidator : AbstractValidator<EUser>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.Email).MinimumLength(3).WithMessage(Messages.NameMinLength);
            RuleFor(e => e.FirstName).MinimumLength(2).WithMessage(Messages.SurnameMinLength);
            RuleFor(e => e.LastName).MinimumLength(2).WithMessage(Messages.UsernameMinLength);
        }
    }
}
