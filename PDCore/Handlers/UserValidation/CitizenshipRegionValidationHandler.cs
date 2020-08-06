using PDCore.Models.Shop;
using PDCore.Models.Shop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers.UserValidation
{
    public class CitizenshipRegionValidationHandler : Handler<User>
    {
        public override void Handle(User user)
        {
            if (user.CitizenshipRegion.TwoLetterISORegionName == "NO")
            {
                throw new UserValidationException("We currently not support Norwegians");
            }

            base.Handle(user);
        }
    }
}
