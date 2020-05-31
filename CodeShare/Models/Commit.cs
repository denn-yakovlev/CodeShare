using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Models
{
    public class Commit : DatabaseEntity
    {
        public DateTime Timestamp { get; set; }

        public IEnumerable<IReference<User>> Contributors { get; set; }
    
        public IEnumerable<Comment> Comments { get; set; }

        public string Content { get; set; }

    }
}
