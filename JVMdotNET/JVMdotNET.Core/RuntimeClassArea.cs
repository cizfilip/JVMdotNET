using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal class RuntimeClassArea
    {
        private IDictionary<string, JavaClass> classes;


        public RuntimeClassArea()
        {
            this.classes = new Dictionary<string, JavaClass>();
        }

        public void LoadClassFile(string fileName)
        {
            var loader = new ClassFileLoader();
            JavaClass newClass;
            using(var reader = File.OpenRead(fileName))
            {
                newClass = loader.Load(reader);
            }
            
            //TODO: spustit staticky constructor
            AddJavaClass(newClass);
        }

        public void AddJavaClass(JavaClass javaClass)
        {
            if (!classes.ContainsKey(javaClass.Name))
            {
                classes.Add(javaClass.Name, javaClass);
            }
        }

        public JavaClass GetClass(string className)
        {
            JavaClass returnClass;
            if (!classes.TryGetValue(className, out returnClass))
            {
                throw new InvalidOperationException(string.Format("Java class {0} not loaded!"));
            }
            if (!returnClass.IsResolved)
            {
                returnClass.Resolve(this);
            }
            return returnClass;
        }
    }
}
