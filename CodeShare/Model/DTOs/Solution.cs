namespace CodeShare.Model.DTOs
{
    public class Solution : DatabaseEntity
    {
        public string ProgrammingLanguageName{ get; set; }

        public string SourceCode { get; set; }
    }
}