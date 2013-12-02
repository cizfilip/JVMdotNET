using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    //TODO: pouzivat ci ne???
    internal sealed class NullInstance : JavaInstance
    {
        private static NullInstance value;

        public static NullInstance Value
        {
            get
            {
                if(value == null)
                {
                    value = new NullInstance();
                }
                return value;
            }
        }


        private NullInstance()
        {

        }

    }
}
