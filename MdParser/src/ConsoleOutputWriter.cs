using System;
using MdParser.interfaces;

namespace MdParser
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void Send(string message)
        {
            Console.WriteLine(message);
        }
    }
}
