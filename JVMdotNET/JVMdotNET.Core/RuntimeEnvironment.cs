using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassLibrary;
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

        private JavaInstance unhandledException;

        public RuntimeEnvironment(RuntimeClassArea classArea)
        {
            this.stack = new Stack<StackFrame>();
            this.classArea = classArea;
            this.unhandledException = null;
        }


        public void LoadConstant(int constantPoolIndex)
        {
            var currentStackFrame = stack.Peek();

            object value = currentStackFrame.ConstantPool.GetItem<ValueConstantPoolItem>(constantPoolIndex).GetValue();
            if (value is string)
            {
                JavaInstance stringInstance = CreateJavaInstance(StringClass.Name);
                stringInstance.Fields[0] = value;
                currentStackFrame.PushToOperandStack(stringInstance);
            }
            else
            {
                currentStackFrame.PushToOperandStack(value);
            }
        }

        public void CreateInstance(string className)
        {
            stack.Peek().PushToOperandStack(CreateJavaInstance(className));
        }

        private JavaInstance CreateJavaInstance(string className)
        {
            JavaClass @class = classArea.GetClass(className);
            classArea.InitializeClass(@class);
            return new JavaInstance(@class);
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


        #region Method invocations
        
        public void PrepareStaticMethodInvocation(object[] parameters, MethodRefConstantPoolItem methodRef)
        {
            JavaClass javaClass = classArea.GetClass(methodRef.Class.Name);
            classArea.InitializeClass(javaClass);

            MethodInfo methodToRun = javaClass.GetMethodInfo(methodRef);

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

        #endregion

        #region Exception handling

        public void SignalException(string exceptionClassName)
        {
            SignalException(CreateJavaInstance(exceptionClassName));
        }
        
        public void SignalException(JavaInstance exception)
        {
            this.unhandledException = exception;
        }

        private void HandleException()
        {
            var currentStackFrame = stack.Peek();

            ExceptionTableEntry[] exceptionTable = currentStackFrame.ExceptionTable;
            int pc = currentStackFrame.PC;

            foreach (var entry in exceptionTable)
            {
                if (pc >= entry.StartPC && pc < entry.EndPC) //The  start_pc is inclusive and  end_pc is exclusive; See JVM 7 spec page 106
                {
                    if (entry.CatchTypeIndex == 0 || CheckExceptionType(currentStackFrame.ConstantPool.GetItem<ClassConstantPoolItem>(entry.CatchTypeIndex).Name))
                    {
                        currentStackFrame.ClearOperandStack();
                        currentStackFrame.PushToOperandStack(unhandledException);
                        currentStackFrame.PC = entry.HandlerPC;
                        unhandledException = null;

                    }
                }
            }

            //propagate exception
            stack.Pop().Unload();
        }

        private bool CheckExceptionType(string catchClassName)
        {

            var catchClass = classArea.GetClass(catchClassName);
            if (unhandledException.JavaClass.IsSubClassOf(catchClass))
            {
                return true;
            }
            return false;
        }
        
        #endregion
               
        #region Fields Manipulation
        
        public void GetStaticFieldValue(FieldRefConstantPoolItem fieldRef)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            classArea.InitializeClass(javaClass);
            object value = javaClass.GetStaticFieldValue(fieldRef.NameAndType.Name);
            stack.Peek().PushToOperandStack(value);
        }

        public void SetStaticFieldValue(FieldRefConstantPoolItem fieldRef, object value)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            classArea.InitializeClass(javaClass);
            javaClass.SetStaticFieldValue(fieldRef.NameAndType.Name, value);
        }

        public void GetFieldValue(FieldRefConstantPoolItem fieldRef, JavaInstance instance)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            int index = javaClass.GetInstanceFieldInfo(fieldRef.NameAndType.Name).Index;
            stack.Peek().PushToOperandStack(instance.Fields[index]);
        }

        public void SetFieldValue(FieldRefConstantPoolItem fieldRef, JavaInstance instance, object value)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            int index = javaClass.GetInstanceFieldInfo(fieldRef.NameAndType.Name).Index;
            instance.Fields[index] = value;
        }

        #endregion

        public JavaInstance ExecuteProgram(MethodInfo mainMethod)
        {
            PrepareMethodInvocation(mainMethod, null, new object[0]);

            Run();

            return unhandledException;
        }

        private void Run()
        {
            while (!stack.IsEmpty())
            {
                stack.Peek().Run();
                if (unhandledException != null)
                {
                    HandleException();
                }
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
