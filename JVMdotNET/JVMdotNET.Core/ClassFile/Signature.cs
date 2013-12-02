using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class Signature
    {
        private static readonly IDictionary<char, Type> SignatureTypes = new Dictionary<char, Type>()
        {
           {'B', typeof(sbyte)},
           {'C', typeof(char)},
           {'D', typeof(double)},
           {'F', typeof(float)},
           {'I', typeof(int)},
           {'J', typeof(long)},
           {'L', typeof(Object)},
           {'S', typeof(short)},
           {'Z', typeof(bool)},
           {'[', typeof(Array)},
        };
    }


}
