using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Exceptions
{
    internal static class ExceptionClassNames
    {
        //TODO: až budou třídy pro exception používat statickou ExceptionClass.Name po vzoru Stringu
        public static readonly string ArithmeticExceptionName = "java/lang/ArithmeticException";
        public static readonly string ArrayIndexOutOfBoundsExceptionName = "java/lang/ArrayIndexOutOfBoundsException";
        public static readonly string NullPointerExceptionName = "java/lang/NullPointerException";
        public static readonly string NegativeArraySizeExceptionName = "java/lang/NegativeArraySizeException";

        
    }
}
