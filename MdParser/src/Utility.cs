using MdParser.models;

namespace MdParser
{
    public static class Utility
    {
        public static string GetErrorMessageCantOpenFile(MarkdownFile file, string message)
        {
            return $"[ERR] {message} at opening the file '{file.FilePath}'";
        }

        public static string GetMessageReadFile(MarkdownFile file)
        {
            return $"[DEBUG] File '{file.FilePath}' read";
        }

        public static string GetImageCount(MarkdownFile file)
        {
            return $"[DEBUG] File '{file.FilePath}' included {file.ImageCount} images";
        }

        public static string GetTableCount(MarkdownFile file)
        {
            return $"[DEBUG] File '{file.FilePath}' included {file.TableCount} tables";
        }

        public static string GetTotalImageCount(int totalImageCount)
        {
            return $"[INFO] Total image count: {totalImageCount}";
        }

        public static string GetTotalTableCount(int totalTableCount)
        {
            return $"[INFO] Total table count: {totalTableCount}";
        }

        public static string GetMessageImageDescriptionMissing(MarkdownFile file, int lineNumber)
        {
            return $"[ERR] File '{file.FilePath}' missing the description of the image in line {lineNumber}";
        }

        public static string GetMessageTableDescriptionMissing(MarkdownFile file, int lineNumber)
        {
            return $"[ERR] File '{file.FilePath}' missing the description of the table in line {lineNumber}";
        }

        public static string GetMessageImageDescriptionMissingEof(MarkdownFile file)
        {
            return $"[ERR] File '{file.FilePath}' missing the description of the image in end of file";
        }

        public static string GetMessageTableDescriptionMissingEof(MarkdownFile file)
        {
            return $"[ERR] File '{file.FilePath}' missing the description of the table in end of file";
        }
    }
}
