﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbraco.Framework
{
    public static class IntExtensions
    {
        public static void Times(this int n, Action<int> action)
        {
            for (int i = 0; i < n; i++)
            {
                action(i);
            }
        }
    }
}
