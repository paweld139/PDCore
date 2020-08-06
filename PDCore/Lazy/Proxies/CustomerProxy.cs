using PDCore.Models.Shop;
using PDCore.Services.Serv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Lazy.Proxies
{
    public class CustomerProxy : Customer
    {
        public override byte[] ProfilePicture
        {
            get
            {
                if (base.ProfilePicture == null)
                {
                    base.ProfilePicture = ProfilePictureService.GetFor(Name);
                }

                return base.ProfilePicture;
            }
        }
    }
}
