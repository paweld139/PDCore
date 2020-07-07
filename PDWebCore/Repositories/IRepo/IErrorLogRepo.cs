﻿using PDCore.Repositories.IRepo;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDWebCore.Repositories.IRepo
{
    public interface ILogRepo : ISqlRepositoryEntityFramework<LogModel>
    {

    }
}
