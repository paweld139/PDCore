﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Helpers.WPF
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        //for example all my pages has title:
        public string Title { get; private set; }
    }
}
