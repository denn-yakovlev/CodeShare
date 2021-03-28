using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class ProgrammingLanguage
    {
        public  string Name { get; }

        private ProgrammingLanguage() { }
        private ProgrammingLanguage(string name)
        {
            Name = name;
        }

        public static ProgrammingLanguage C { get; } = new ProgrammingLanguage("C");
        public static ProgrammingLanguage Cpp { get; } = new ProgrammingLanguage("C++");
        public static ProgrammingLanguage CSharp { get; } = new ProgrammingLanguage("C#");
        public static ProgrammingLanguage Go { get; } = new ProgrammingLanguage("Go");
        public static ProgrammingLanguage Java { get; } = new ProgrammingLanguage("Java");
        public static ProgrammingLanguage Kotlin { get; } = new ProgrammingLanguage("Kotlin");
        public static ProgrammingLanguage Python { get; } = new ProgrammingLanguage("Python");
    }
}
