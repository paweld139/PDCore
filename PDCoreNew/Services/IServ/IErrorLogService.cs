using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCoreNew.Services.IServ
{
    public interface IErrorLogService
    {
        string Log(Exception exception);
    }
}
