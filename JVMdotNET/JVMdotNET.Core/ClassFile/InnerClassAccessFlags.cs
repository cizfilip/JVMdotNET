using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    [Flags]
    internal enum InnerClassAccessFlags : ushort
    {
        Public = 0x0001, // Marked or implicitly public in source.
        Private = 0x0002, // Marked private in source.
        Protected = 0x0004, // Marked protected in source.
        Static = 0x0008, // Marked or implicitly static in source.
        Final = 0x0010, // Marked final in source.
        Interface = 0x0200, // Was an interface in source.
        Abstract = 0x0400, // Marked or implicitly abstract in source.
        Synthetic = 0x1000, // Declared synthetic; not present in the source code.
        Annotation = 0x2000, // Declared as an annotation type.
        Enum = 0x4000 // Declared as an enum type.
    }
}
