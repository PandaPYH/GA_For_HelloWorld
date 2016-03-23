using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            GA ga = new GA("Hello World");
            ga.Run();
            Console.Read();
        }
    }
}
