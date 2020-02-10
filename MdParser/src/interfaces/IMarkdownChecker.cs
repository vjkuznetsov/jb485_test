using MdParser.models;

namespace MdParser.interfaces
{
    public interface IMarkdownChecker
    {
        MarkdownFile Check(MarkdownFile file);
    }
}
