﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IEntity
    {
        bool IsValid();

        int Id { get; set; }
    }
}
