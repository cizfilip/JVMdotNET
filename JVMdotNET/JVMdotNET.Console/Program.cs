using JVMdotNET.Core;
using JVMdotNET.Core.ClassFile.Signature;
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
            string debugPath = @"C:\\Users\\Filip\\SkyDrive\\Eclipse\\RUNKnapsack\\bin\\runknapsack";

            List<string> classFiles = new List<string>();
            classFiles.Add(Path.Combine(debugPath, "RUNKnapsack.class"));
            classFiles.Add(Path.Combine(debugPath, "Knapsack.class"));
            classFiles.Add(Path.Combine(debugPath, "Algorithm.class"));
            
            //classFiles.Add(Path.Combine(debugPath, "Pokus.class"));
            //classFiles.Add(Path.Combine(debugPath, "PokusChild.class"));
            //classFiles.Add(Path.Combine(debugPath, "PokInterface.class"));


            var jvm = new JavaVirtualMachine(classFiles.ToArray());

            jvm.Run();


            Console.ReadKey();
        }
    }
}
