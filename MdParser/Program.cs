using System;
using MdParser.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MdParser
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("[DEBUG] Start application");

            var serviceProvider = ContainerBuilder.CreateContainer();

            var runner = serviceProvider.GetService<IRunner>();

            runner.Run(args);

            Console.WriteLine("[DEBUG] Stop application");
        }
    }
}
