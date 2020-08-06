using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PDCore.Validators
{
    public class SocialSecurityNumberValidator
    {
        public bool Validate(string socialSecurityNumber, RegionInfo citizenshipRegion)
        {
            _ = socialSecurityNumber;

            _ = citizenshipRegion;

            return true;
        }
    }
}
