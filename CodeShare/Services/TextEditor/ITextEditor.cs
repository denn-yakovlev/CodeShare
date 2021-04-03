using CodeShare.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.TextEditor
{
    public interface ITextEditor
    {
        public Solution Solution { get; }
    }
}
