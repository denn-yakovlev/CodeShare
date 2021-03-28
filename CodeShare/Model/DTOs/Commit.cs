using System;
using System.Collections.Generic;

namespace CodeShare.Model.DTOs
{
    public class Commit : DatabaseEntity
    {
        public DateTime Timestamp { get; set; }

        public IEnumerable<IReference<User>> Contributors { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public string Content { get; set; }

    }
}
