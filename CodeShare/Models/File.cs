﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Models
{
    public class File : DatabaseEntity
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public IEnumerable<Commit> Commits { get; set; }
    }
}
