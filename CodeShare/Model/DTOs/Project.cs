using System.Collections.Generic;

namespace CodeShare.Model.DTOs
{
    public class Project : DatabaseEntity
    {
        public string Name { get; set; }

        public IReference<User> Owner { get; set; }

        public string Description { get; set; }

        public IEnumerable<File> Files { get; set; }

    }
}
