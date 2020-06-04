using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Factories.IFac
{
    public interface IMessageCreator
    {
        string CreateMessage(object result);
    }
}
