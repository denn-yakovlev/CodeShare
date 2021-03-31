﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class Task : IEquatable<Task>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Equals([AllowNull] Task other) => 
            other != null && other.Name == Name && other.Description == Description;
    }
}
