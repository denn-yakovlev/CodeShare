namespace CodeShare.Model.DTOs
{
    public class Session : DatabaseEntity
    {
        public Task? Task { get; set; }

        public Solution CurrentSolution { get; set; }
    }
}
