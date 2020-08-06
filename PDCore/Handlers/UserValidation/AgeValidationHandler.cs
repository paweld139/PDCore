using PDCore.Models.Shop;
using PDCore.Models.Shop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers.UserValidation
{
    public class AgeValidationHandler : Handler<User>
    {
        public override void Handle(User user)
        {
            if (user.Age < 18)
            {
                throw new UserValidationException("You have to be 18 years or older");
            }

            base.Handle(user);
        }
    }
}
