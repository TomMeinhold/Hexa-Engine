﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.UI.Events
{
    public class FocusEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}