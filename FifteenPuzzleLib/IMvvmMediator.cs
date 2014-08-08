using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenPuzzleLib
{
    public interface IMvvmMediator<T>
    {
        bool Query(string prompt, out T value);
    }
}
