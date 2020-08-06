using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Models.Shop.Exceptions
{
    public class UserValidationException : SystemException
    {
        public UserValidationException(string message) : base(message)
        {
        }
    }
}
