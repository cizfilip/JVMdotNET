using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal class RuntimeEnvironment
    {
        private Stack<StackFrame> stack;
        private RuntimeClassArea classArea;

        //exception for exception propagation
        private JavaInstance exception;

        public RuntimeEnvironment(RuntimeClassArea classArea)
        {
            this.stack = new Stack<StackFrame>();
            this.classArea = classArea;
            this.exception = null;
        }
        
        public void PrepareReturn(object returnValue)
        {
            stack.Pop().Unload();
            var frame = stack.Peek(); // Java Main method must return void! - otherwise here .Net throws InvalidOperationException(Stack is empty)
            frame.PushToOperandStack(returnValue);
        }

        public void PrepareReturnVoid()
        {
            stack.Pop().Unload();
        }

        public void PrepareStaticMethodInvocation(object[] parameters, MethodRefConstantPoolItem methodRef)
        {
            MethodInfo methodToRun = classArea.GetClass(methodRef.Class.Name).GetMethodInfo(methodRef);

            if (!methodToRun.AccessFlags.HasFlag(MethodAccessFlags.Static))
            {
                throw new InvalidOperationException(string.Format("Method {0} on class {1} was caled using invokestatic, but it is not static method!", methodToRun.Name, methodToRun.Class.Name));
            }

            if (methodToRun.IsInitializationMethod)
            {
                throw new InvalidOperationException(string.Format("Method {0} on class {1} was caled using invokestatic, but it is an initialization method!", methodToRun.Name, methodToRun.Class.Name));
            }

            PrepareMethodInvocation(methodToRun, null, parameters);
        }

        public void PrepareVirtualOrInterfaceMethodInvocation(JavaInstance instance, object[] parameters, MethodRefConstantPoolItem methodRef)
        {
            MethodInfo methodToRun = instance.JavaClass.GetMethodInfo(methodRef);

            if (methodToRun.IsInitializationMethod)
            {
                throw new InvalidOperationException(string.Format("Method {0} on class {1} was caled using invokevirtual, but it is an initialization method!", methodToRun.Name, methodToRun.Class.Name));
            }

            PrepareMethodInvocation(methodToRun, instance, parameters);
        }

        public void PrepareSpecialMethodInvocation(JavaInstance instance, object[] parameters, MethodRefConstantPoolItem methodRef)
        {
            MethodInfo methodToRun = classArea.GetClass(methodRef.Class.Name).GetMethodInfo(methodRef);

            PrepareMethodInvocation(methodToRun, instance, parameters);
        }

        private void PrepareMethodInvocation(MethodInfo methodToRun, JavaInstance instance, object[] parameters)
        {
            if (methodToRun.IsNative)
            {
                NativeMethodInfo nativeMethod = (NativeMethodInfo)methodToRun;
                object returnValue = nativeMethod.Implementation(instance, parameters, this);
                if (returnValue != null)
                {
                    stack.Peek().PushToOperandStack(returnValue);
                }
            }
            else
            {
                StackFrame newFrame = new StackFrame(methodToRun, this);
                newFrame.InitLocals(instance, parameters);
                stack.Push(newFrame);
            }
        }

        

        public void PrepareExceptionHandling(JavaInstance exception)
        {
            stack.Pop().Unload();
            this.exception = exception;
        }

        public void ExceptionHandled()
        {
            this.exception = null;
        }

        public JavaInstance CreateInstance(string className)
        {
            JavaClass @class = classArea.GetClass(className);
            
            return new JavaInstance(@class);
        }

        public JavaClass GetClass(string className)
        {
            return classArea.GetClass(className);
        }

        //TODO: static constructors.... maybe not this way
        //public bool EnsureClassInitialized(ClassConstantPoolItem classRef)
        //{
        //    var javaClass = classArea.GetClass(classRef.Name);
        //    if (!javaClass.IsInitialized)
        //    {
        //        javaClass.IsInitialized = true;

        //        MethodInfo classConstructor;
        //        if (javaClass.TryGetClassConstructor(out classConstructor))
        //        {
        //            PrepareMethodInvocation(classConstructor, null, new object[0]);
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        #region Fields Manipulation
        
        public object GetStaticFieldValue(FieldRefConstantPoolItem fieldRef)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            return javaClass.GetStaticFieldValue(fieldRef.NameAndType.Name);
        }

        public void SetStaticFieldValue(FieldRefConstantPoolItem fieldRef, object value)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            javaClass.SetStaticFieldValue(fieldRef.NameAndType.Name, value);
        }

        public object GetFieldValue(FieldRefConstantPoolItem fieldRef, JavaInstance instance)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            int index = javaClass.GetInstanceFieldInfo(fieldRef.NameAndType.Name).Index;
            return instance.Fields[index];
        }

        public void SetFieldValue(FieldRefConstantPoolItem fieldRef, JavaInstance instance, object value)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            int index = javaClass.GetInstanceFieldInfo(fieldRef.NameAndType.Name).Index;
            instance.Fields[index] = value;
        }

        #endregion

        public void ExecuteProgram(MethodInfo mainMethod)
        {
            StackFrame newFrame = new StackFrame(mainMethod, this);
            stack.Push(newFrame);

            Run();

            if (exception != null)
            {
                //TODO: lepsi handlovani konce s exception
                throw new InvalidOperationException(string.Format("Java program ended with exception {0}", exception.JavaClass.Name));
            }
        }

        private void Run()
        {
            while (!stack.IsEmpty())
            {
                stack.Peek().Run(exception);
            }
        }
    }


    internal static class StackStackFrameExtensions
    {
        public static bool IsEmpty(this Stack<StackFrame> stack)
        {
            return stack.Count == 0;
        }
    }
}
