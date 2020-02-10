using MdParser.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MdParser
{
    public static class ContainerBuilder
    {
        public static ServiceProvider CreateContainer()
        {
            var container = new ServiceCollection()
                .AddSingleton<IInputReader, InputReaderTextFile>()
                .AddSingleton<IOutputWriter, ConsoleOutputWriter>()
                .AddSingleton<IMarkdownChecker, MarkdownChecker>()
                .AddSingleton<IRunner, Runner>()
                .BuildServiceProvider();

            return container;
        }
    }
}
