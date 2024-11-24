using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit.Services
{
    public interface IConsoleService
    {
        void Write(string message);
        string Read();
    }
    public class ConsoleService : IConsoleService
    {
        public string Read()
        {
           return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
