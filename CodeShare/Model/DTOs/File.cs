using System.Collections.Generic;

namespace CodeShare.Model.DTOs
{
    public class File : DatabaseEntity
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public IEnumerable<Commit> Commits { get; set; }
    }
}
