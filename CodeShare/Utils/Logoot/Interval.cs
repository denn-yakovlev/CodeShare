﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public readonly struct Interval<T>
    {
        public T Begin { get; }

        public T End { get; }

        public Interval(T begin, T end)
        {
            Begin = begin;
            End = end;
        }
    }
}
