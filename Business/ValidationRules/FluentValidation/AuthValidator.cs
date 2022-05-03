using Business.Constants;
using Core.Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthValidator : AbstractValidator<UserForLoginDto>
    {
        public AuthValidator()
        {
            RuleFor(e => e.Email).MinimumLength(3).WithMessage(Messages.NameMinLength);
        }
    }
}
