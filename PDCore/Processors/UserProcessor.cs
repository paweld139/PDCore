using PDCore.Handlers.UserValidation;
using PDCore.Models.Shop;
using PDCore.Models.Shop.Exceptions;

namespace PDCore.Processors
{
    public class UserProcessor
    {
        public bool Register(User user)
        {
            try
            {
                var handler = new SocialSecurityNumberValidatorHandler();

                handler.SetNext(new AgeValidationHandler())
                    .SetNext(new NameValidationHandler())
                    .SetNext(new CitizenshipRegionValidationHandler());

                handler.Handle(user);
            }
            catch (UserValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
