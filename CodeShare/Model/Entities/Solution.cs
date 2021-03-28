using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class Solution
    {
        public ProgrammingLanguage Language { get; set; }

        public string SourceCode { get; set; }
    }
}
