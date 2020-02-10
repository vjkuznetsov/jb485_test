using System.Collections.Generic;

namespace MdParser.models
{
    // MarkdownFile object
    public class MarkdownFile
    {
        public string FilePath { get; set; }

        // file data, which was separated into rows
        public IEnumerable<string> Lines { get; set; }

        // messages that were received when the file was processed
        public IEnumerable<string> Messages { get; set; } = new List<string>();

        public int ImageCount { get; set; }

        public int TableCount { get; set; }

        public bool ErrorStatus { get; set; }
    }
}
