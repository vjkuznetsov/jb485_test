using System.Collections.Generic;

namespace MdParser.interfaces
{
    public interface IRunner
    {
        void Run(IEnumerable<string> filepaths);
    }
}
