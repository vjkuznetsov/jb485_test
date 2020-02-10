using System.Linq;
using System.Text.RegularExpressions;
using MdParser.interfaces;
using MdParser.models;

namespace MdParser
{
    public class MarkdownChecker : IMarkdownChecker
    {
        private readonly IOutputWriter _outputWriter;

        private const string ImagePattern = @"^!\[(.*?)\]\((.*?)\)";
        private const string TableRowPattern = @"(.*?)\|(.*?)";
        private const string TableDelimiterPattern = @"(.*?)\-(.*?)\|(.*?)\-(.*?)";
        private const string ImageDescriptionExpectedPattern = @"^Рисунок";
        private const string TableDescriptionExpectedPattern = @"^Таблица";

        private readonly Regex _imageRegex = new Regex(ImagePattern);
        private readonly Regex _tableRowRegex = new Regex(TableRowPattern);
        private readonly Regex _tableDelimiterRegex = new Regex(TableDelimiterPattern);
        private readonly Regex _imageDescriptionExpectedRegex = new Regex(ImageDescriptionExpectedPattern);
        private readonly Regex _tableDescriptionExpectedRegex = new Regex(TableDescriptionExpectedPattern);

        public MarkdownChecker(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public MarkdownFile Check(MarkdownFile file)
        {
            if (file.ErrorStatus)
            {
                return new MarkdownFile();
            }

            var lineNumber = 0;
            var imageCounter = 0;
            var tableCounter = 0;
            var previousLineStatus = PreviousLine.Row;

            foreach (var line in file.Lines)
            {
                lineNumber++;

                if (!string.IsNullOrWhiteSpace(line)) {
                    
                    if (previousLineStatus == PreviousLine.Image)
                    {
                        if (!_imageDescriptionExpectedRegex.IsMatch(line))
                        {
                            var errorMessage = Utility.GetMessageImageDescriptionMissing(file, lineNumber);
                            
                            _outputWriter.Send(errorMessage);

                            file = AddMessage(file, errorMessage);
                        }

                        previousLineStatus = PreviousLine.Row;
                    }

                    if (previousLineStatus == PreviousLine.EndOfTable)
                    {
                        if (!_tableDescriptionExpectedRegex.IsMatch(line))
                        {
                            var errorMessage = Utility.GetMessageTableDescriptionMissing(file, lineNumber);

                            _outputWriter.Send(errorMessage);

                            file = AddMessage(file, errorMessage);
                        }

                        previousLineStatus = PreviousLine.Row;
                    }
                }
                

                if (line != null && _imageRegex.IsMatch(line))
                {
                    imageCounter++;
                    previousLineStatus = PreviousLine.Image;
                    continue;
                }

                switch (previousLineStatus)
                {
                    case PreviousLine.Row:
                        if (line != null && _tableRowRegex.IsMatch(line))
                        {
                            previousLineStatus = PreviousLine.TableHeader;
                        };
                        break;

                    case PreviousLine.TableHeader:
                        if (line != null && _tableDelimiterRegex.IsMatch(line))
                        {
                            tableCounter++;
                            previousLineStatus = PreviousLine.TableDelimiterOrTableRow;
                        }
                        else
                        {
                            previousLineStatus = PreviousLine.Row;
                        }
                        break;

                    case PreviousLine.TableDelimiterOrTableRow:
                        if (line != null && !_tableRowRegex.IsMatch(line))
                            // true conditions in regex - table continues,
                            // false conditions - table ended. Expected blank line or description/error
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                previousLineStatus = PreviousLine.EndOfTable;
                            }
                            else
                            {
                                if (!_tableDescriptionExpectedRegex.IsMatch(line))
                                {
                                    var errorMessage = Utility.GetMessageTableDescriptionMissing(file, lineNumber);

                                    _outputWriter.Send(errorMessage);

                                    file = AddMessage(file, errorMessage);
                                }
                                previousLineStatus = PreviousLine.Row;
                            }
                            
                        }
                        break;
                }

            }
            file.ImageCount = imageCounter;
            file.TableCount = tableCounter;

            // check after end of file

            switch (previousLineStatus)
            {
                case PreviousLine.Image:
                {
                    var errorMessage = Utility.GetMessageImageDescriptionMissingEof(file);
                    _outputWriter.Send(errorMessage);
                    file = AddMessage(file, errorMessage);
                    break;
                }
                case PreviousLine.EndOfTable:
                case PreviousLine.TableDelimiterOrTableRow:
                {
                    var errorMessage = Utility.GetMessageTableDescriptionMissingEof(file);
                    _outputWriter.Send(errorMessage);
                    file = AddMessage(file, errorMessage);
                    break;
                }
            }

            _outputWriter.Send(Utility.GetImageCount(file));
            _outputWriter.Send(Utility.GetTableCount(file));

            return file;
        }

        private static MarkdownFile AddMessage(MarkdownFile file, string message)
        {
            var messages = file.Messages.ToList();

            messages.Add(message);

            file.Messages = messages;

            return file;
        }
    }
}
