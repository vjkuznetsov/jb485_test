using MdParser.models;

namespace MdParser.interfaces
{
    public interface IInputReader
    {
        MarkdownFile Read(MarkdownFile file);
    }
}
