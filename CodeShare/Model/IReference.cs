using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model
{
    public interface IReference<T>
    {
        T ReferencedItem { get; }
    }
}
