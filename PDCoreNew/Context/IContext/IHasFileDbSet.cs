﻿using PDCoreNew.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Context.IContext
{
    public interface IHasFileDbSet
    {
        DbSet<FileModel> File { get; set; }
    }
}
