using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Models
{
    public interface IReference<T>
    {
        T ReferencedItem { get; }
    }
}
