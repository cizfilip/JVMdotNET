using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    [Flags]
    internal enum FieldAccessFlags : ushort
    {
        Public = 0x0001, //Declared public; may be accessed from outside its package.
        Private = 0x0002, //Declared private; usable only within the defining class.
        Protected = 0x0004, //Declared protected; may be accessed within subclasses.
        Static = 0x0008, //Declared static.
        Final = 0x0010, //Declared final; never directly assigned to after object construction (JLS §17.5).
        Volatile = 0x0040, //Declared volatile; cannot be cached.
        Transient = 0x0080, //Declared transient; not written or read by a persistent object manager.
        Synthetic = 0x1000, //Declared synthetic; not present in the source code.
        Enum = 0x4000 //Declared as an element of an enum.
    }
}
