using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Loader;
using JVMdotNET.Core.ClassLibrary;
using JVMdotNET.Core.ClassLibrary.Exceptions;
using JVMdotNET.Core.ClassLibrary.Io;
using JVMdotNET.Core.Exceptions;
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

        private RuntimeEnvironment classInitializationEnvironment;

        public RuntimeClassArea()
        {
            this.classes = new Dictionary<string, JavaClass>();
            this.classInitializationEnvironment = new RuntimeEnvironment(this);
        }

        public void LoadClassFile(string fileName)
        {
            var loader = new ClassFileLoader();
            JavaClass newClass;
            using(var reader = File.OpenRead(fileName))
            {
                newClass = loader.Load(reader);
            }
            
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
                throw new ClassNotFoundException(string.Format("Java class {0} not loaded!", className));
            }
            if (!returnClass.IsResolved)
            {
                returnClass.Resolve(this);
            }
            return returnClass;
        }

        public void InitializeClass(JavaClass javaClass)
        {
            if (!javaClass.IsResolved)
            {
                throw new InvalidOperationException(string.Format("Class {0} initialization occurred before resolution!", javaClass.Name));
            }

            if (javaClass.IsInitialized)
            {
                return;
            }

            if (javaClass.SuperClass != null && !javaClass.SuperClass.IsInitialized)
            {
                InitializeClass(javaClass.SuperClass);
            }

            javaClass.IsInitialized = true;

            MethodInfo classConstructor;
            if (javaClass.TryGetClassConstructor(out classConstructor))
            {
                var exception = classInitializationEnvironment.ExecuteProgram(classConstructor);
                if (exception != null)
                {
                    //TODO: propagate ExceptionInInitializerError to JVM
                    throw new InvalidOperationException("ExceptionInInitializerError");
                }
            }
        }

        public MethodInfo FindMainMethod()
        {
            int methodsFound = 0;
            MethodInfo mainMethod = null;

            foreach (var javaClass in classes.Values)
            {
                MethodInfo mainCandidate;
                if (javaClass.TryGetMethodInfo("main([Ljava/lang/String;)V", out mainCandidate) &&
                    mainCandidate.AccessFlags.HasFlag(MethodAccessFlags.Public) &&
                    mainCandidate.AccessFlags.HasFlag(MethodAccessFlags.Static))
                {
                    methodsFound++;
                    mainMethod = mainCandidate;
                }
            }

            switch (methodsFound)
            {
                case 0:
                    throw new InvalidOperationException("main method not found.");
                case 1:
                    return mainMethod;
                default:
                    throw new InvalidOperationException("More than one main method found!");
            }
        }

        public void LoadSupportedClassFromJavaClassLibrary()
        {
            AddJavaClass(new ObjectClass());
            AddJavaClass(new StringClass());

            //System.out.print ... (limited support)
            AddJavaClass(new SystemClass());
            AddJavaClass(new PrintStreamClass());

            //Exceptions
            AddJavaClass(new ThrowableClass());
            AddJavaClass(new ExceptionClass());
            AddJavaClass(new RuntimeExceptionClass());
            AddJavaClass(new ArithmeticExceptionClass());
            AddJavaClass(new ArrayIndexOutOfBoundsExceptionClass());
            AddJavaClass(new NegativeArraySizeExceptionClass());
            AddJavaClass(new NullPointerExceptionClass());
        }

    }
}
