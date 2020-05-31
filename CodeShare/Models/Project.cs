using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Models
{
    public class Project : DatabaseEntity
    {
        public string Name { get; set; }

        public IReference<User> Owner { get; set; }

        public string Description { get; set; }
        
        public IEnumerable<File> Files { get; set; }

        
    }
}
