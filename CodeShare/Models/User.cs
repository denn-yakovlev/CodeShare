using System.Collections.Generic;

namespace CodeShare.Models
{
    public class User : DatabaseEntity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        // public IProjectToRoleMapping RolesInProjects { get; set; }
    }
}