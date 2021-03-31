namespace CodeShare.Model.DTOs
{
    public class PermissionsInProject : DatabaseEntity
    {
        public Reference<Task> Project { get; set; }

        public Reference<Permission> Permission { get; set; }
    }
}