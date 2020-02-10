using System.Collections.Generic;
using MdParser.interfaces;
using MdParser.models;

namespace MdParser
{
    public class Runner : IRunner
    {
        private readonly IInputReader _inputReader;
        private readonly IOutputWriter _outputWriter;
        private readonly IMarkdownChecker _markdownChecker;

        public Runner(
            IInputReader inputReader,
            IOutputWriter outputWriter,
            IMarkdownChecker markdownChecker)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
            _markdownChecker = markdownChecker;
        }

        public void Run(IEnumerable<string> filepaths)
        {
            var imageTotalCounter = 0;
            var tableTotalCounter = 0;
            
            foreach (var filepath in filepaths)
            {
                var file = new MarkdownFile()
                {
                    FilePath = filepath
                };
                _inputReader.Read(file);
                _markdownChecker.Check(file);

                imageTotalCounter += file.ImageCount;
                tableTotalCounter += file.TableCount;

            }

            _outputWriter.Send(Utility.GetTotalImageCount(imageTotalCounter));
            _outputWriter.Send(Utility.GetTotalTableCount(tableTotalCounter));
        }
    }
}
