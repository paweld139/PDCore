using PDCore.Models.Shop;
using PDCore.Models.Shop.Exceptions;
using PDCore.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers.UserValidation
{
    public class SocialSecurityNumberValidatorHandler : Handler<User>
    {
        private readonly SocialSecurityNumberValidator socialSecurityNumberValidator
            = new SocialSecurityNumberValidator();

        public override void Handle(User request)
        {
            if (!socialSecurityNumberValidator.Validate(request.SocialSecurityNumber, request.CitizenshipRegion))
            {
                throw new UserValidationException("Social security number could not be valid");
            }

            base.Handle(request);
        }
    }
}
