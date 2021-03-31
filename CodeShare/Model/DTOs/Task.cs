using System.Collections.Generic;

namespace CodeShare.Model.DTOs
{
    public class Task : DatabaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
