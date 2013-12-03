using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.TypeSystem
{
    internal sealed class NullReference : JavaReference
    {
        private static NullReference value;

        public static NullReference Value
        {
            get
            {
                if(value == null)
                {
                    value = new NullReference();
                }
                return value;
            }
        }


        private NullReference()
        {

        }

    }
}
