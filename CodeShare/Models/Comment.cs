using System;
using System.Collections.Generic;

namespace CodeShare.Models
{
    public class Comment : DatabaseEntity
    {
        public DateTime Timestamp { get; set; }

        public IReference<User> Author { get; set; }

        public string Text { get; set; }
    }
}