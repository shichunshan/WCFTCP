using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcf
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Press <ENTER> to start the service.");
            System.Console.ReadLine();
            MyHost.Open();
            System.Console.WriteLine("The service is started.");
            System.Console.WriteLine("Press <ENTER> to stop the service.");
            System.Console.ReadLine();
            MyHost.Close();
        }
    }
}
