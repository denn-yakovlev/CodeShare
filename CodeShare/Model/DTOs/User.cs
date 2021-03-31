﻿using System.Collections.Generic;

namespace CodeShare.Model.DTOs
{
    public class User : DatabaseEntity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public IDictionary<IReference<Task>, IEnumerable<IReference<Permission>>>
            RolesInProjects
        { get; set; }
    }
}