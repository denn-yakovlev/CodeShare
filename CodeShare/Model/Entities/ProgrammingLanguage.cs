using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class ProgrammingLanguage
    {
        public string Name { get; } = string.Empty;
        public string DisplayName { get; } = string.Empty;

        public static ProgrammingLanguage C { get; } = new ProgrammingLanguage(cName, "C");
        public static ProgrammingLanguage Cpp { get; } = new ProgrammingLanguage(cppName, "C++");
        public static ProgrammingLanguage CSharp { get; } = new ProgrammingLanguage(csharpName, "C#");
        public static ProgrammingLanguage Go { get; } = new ProgrammingLanguage(goName, "Go");
        public static ProgrammingLanguage Java { get; } = new ProgrammingLanguage(javaName, "Java");
        public static ProgrammingLanguage Kotlin { get; } = new ProgrammingLanguage(kotlinName, "Kotlin");
        public static ProgrammingLanguage Python { get; } = new ProgrammingLanguage(pyName, "Python");

        public static ProgrammingLanguage GetByName(string languageName) => languages[languageName];

        private ProgrammingLanguage() { }

        private ProgrammingLanguage(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        private const string cName = "c";
        private const string cppName = "cpp";
        private const string csharpName = "csharp";
        private const string goName = "go";
        private const string javaName = "java";
        private const string kotlinName = "kotlin";
        private const string pyName = "python";

        private static Dictionary<string, ProgrammingLanguage> languages =
            new Dictionary<string, ProgrammingLanguage>()
            {
                [cName] = C,
                [cppName] = Cpp,
                [csharpName] = CSharp,
                [goName] = Go,
                [javaName] = Java,
                [kotlinName] = Kotlin,
                [pyName] = Python
            };
    }
}
