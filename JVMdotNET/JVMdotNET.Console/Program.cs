using JVMdotNET.Core;
using JVMdotNET.Core.ClassFile.Signature;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JVMdotNET.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var jvm = new JavaVirtualMachine(args);
                jvm.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
