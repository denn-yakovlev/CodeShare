namespace CodeShare.Model.DTOs
{
    public class PermissionsInProject : DatabaseEntity
    {
        public Reference<Project> Project { get; set; }

        public Reference<Permission> Permission { get; set; }
    }
}