﻿using JVMdotNET.Core.ClassFile;
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


        public RuntimeEnvironment(RuntimeClassArea classArea)
        {
            this.stack = new Stack<StackFrame>();
            this.classArea = classArea;
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

        public void PrepareMethodInvocation()
        {

        }

        public void PrepareExceptionHandling()
        {

        }

        public JavaInstance CreateInstance(string className)
        {
            JavaClass @class = classArea.GetClass(className);
            
            return new JavaInstance(@class);
        }

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
            int index = javaClass.GetFieldInfo(fieldRef.NameAndType.Name).Index;
            return instance.Fields[index];
        }

        public void SetFieldValue(FieldRefConstantPoolItem fieldRef, JavaInstance instance, object value)
        {
            var javaClass = classArea.GetClass(fieldRef.Class.Name);
            int index = javaClass.GetFieldInfo(fieldRef.NameAndType.Name).Index;
            instance.Fields[index] = value;
        }


        public void ExecuteProgram(JavaClass @class, MethodInfo mainMethod)
        {
            StackFrame newFrame = new StackFrame(@class, mainMethod);
            stack.Push(newFrame);

            Run();
        }

        private void Run()
        {
            StackFrame currentFrame;
            while (!stack.IsEmpty())
            {
                currentFrame = stack.Peek();
                currentFrame.Run(this);
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
