using System;
using System.IO;
using System.Linq;
using MdParser.interfaces;
using MdParser.models;

namespace MdParser
{
    public class InputReaderTextFile : IInputReader
    {
        private readonly IOutputWriter _outputWriter;

        public InputReaderTextFile(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public MarkdownFile Read(MarkdownFile file)
        {
            string message;
            try
            {
                file.Lines = File.ReadAllLines(file.FilePath);
                message = Utility.GetMessageReadFile(file);
            }
            catch (Exception exc)
            {
                file.Lines = Enumerable.Empty<string>();
                file.ErrorStatus = true;
                message = Utility.GetErrorMessageCantOpenFile(file, exc.Message);
            }

            _outputWriter.Send(message);
            return file;
        }
    }
}
