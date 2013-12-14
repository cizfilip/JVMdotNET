using JVMdotNET.Core.Bytecode;
using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassFile.Signature;
using JVMdotNET.Core.ClassLibrary;
using JVMdotNET.Core.ClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal class StackFrame
    {
        private int pc;
        public int PC
        {
            get
            {
                return pc;
            }
            set
            {
                pc = value;
            }
        }

        private bool wasWideInstruction;

        private object[] locals;
        private Stack<object> operandStack;

        private JavaClass classObject;
        private MethodInfo method;

        private CodeAttribute codeInfo;
        private RuntimeEnvironment environment;

        public ExceptionTableEntry[] ExceptionTable
        {
            get
            {
                return codeInfo.ExceptionTable;
            }
        }
        public ConstantPoolItemBase[] ConstantPool
        {
            get
            {
                return classObject.ConstantPool;
            }
        }


        public StackFrame(MethodInfo method, RuntimeEnvironment environment)
        {
            this.pc = 0;
            this.wasWideInstruction = false;
            this.classObject = method.Class;
            this.method = method;

            this.codeInfo = method.Code;
            this.locals = new object[codeInfo.MaxLocals];
            this.operandStack = new Stack<object>(codeInfo.MaxStack);
            this.environment = environment;
        }

        public void Unload()
        {
            this.locals = null;
            this.operandStack = null;
            this.classObject = null;
            this.method = null;
            this.environment = null;
            this.codeInfo = null;
        }

        public void PushToOperandStack(object value)
        {
            operandStack.Push(value);
        }

        public void ClearOperandStack()
        {
            operandStack.Clear();
        }

        public void InitLocals(JavaInstance thisInstance, object[] arguments)
        {
            int localsIndex = 0;

            if (thisInstance != null)
            {
                locals[0] = thisInstance;
                localsIndex = 1;
            }

            foreach (var argument in arguments)
            {
                if (localsIndex >= locals.Length)
                {
                    throw new InvalidOperationException("Method arguments is too long!");
                }
                locals[localsIndex] = argument;
                if (argument is long || argument is double)
                {
                    localsIndex++;
                }
                localsIndex++;
            }
        }

        public void Run()
        {
            byte[] code = codeInfo.Code;
            int codeLength = code.Length;

            //helper variables
            int index = 0;
            int newPC = 0;
            object[] array = null;
            object value = null;
            int iValue = 0;
            long lValue = 0;
            float fValue = 0;
            double dValue = 0;
            byte atype = 0;
            JavaInstance instance = null;
            MethodRefConstantPoolItem methodRef = null;

            var instruction = (Instruction)code.ReadByte(ref pc);
            
            switch (instruction)
            {
                case Instruction.nop:
                    return;
                case Instruction.aconst_null:
                    operandStack.Push(null);
                    return;
                case Instruction.iconst_m1:
                    operandStack.Push((int)-1);
                    return;
                case Instruction.iconst_0:
                    operandStack.Push((int)0);
                    return;
                case Instruction.iconst_1:
                    operandStack.Push((int)1);
                    return;
                case Instruction.iconst_2:
                    operandStack.Push((int)2);
                    return;
                case Instruction.iconst_3:
                    operandStack.Push((int)3);
                    return;
                case Instruction.iconst_4:
                    operandStack.Push((int)4);
                    return;
                case Instruction.iconst_5:
                    operandStack.Push((int)5);
                    return;
                case Instruction.lconst_0:
                    operandStack.Push((long)0);
                    return;
                case Instruction.lconst_1:
                    operandStack.Push((long)1);
                    return;
                case Instruction.fconst_0:
                    operandStack.Push((float)0.0);
                    return;
                case Instruction.fconst_1:
                    operandStack.Push((float)1.0);
                    return;
                case Instruction.fconst_2:
                    operandStack.Push((float)2.0);
                    return;
                case Instruction.dconst_0:
                    operandStack.Push((double)0.0);
                    return;
                case Instruction.dconst_1:
                    operandStack.Push((double)1.0);
                    return;
                case Instruction.bipush:
                    operandStack.Push((int)code.ReadSByte(ref pc));
                    return;
                case Instruction.sipush:
                    operandStack.Push((int)code.ReadShort(ref pc));
                    return;

                //LDC instruction throws if used for class, method type or method handle constant pool items
                case Instruction.ldc:
                    index = code.ReadByte(ref pc);
                    environment.LoadConstant(index);
                    return;
                case Instruction.ldc_w:
                    index = code.ReadShort(ref pc);
                    environment.LoadConstant(index);
                    return;
                case Instruction.ldc2_w:
                    index = code.ReadShort(ref pc);
                    environment.LoadConstant(index);
                    return;

                case Instruction.iload:
                case Instruction.lload:
                case Instruction.fload:
                case Instruction.dload:
                case Instruction.aload:
                    index = ReadWithWideCheck(code);
                    operandStack.Push(locals[index]);
                    return;
                case Instruction.iload_0:
                case Instruction.lload_0:
                case Instruction.fload_0:
                case Instruction.dload_0:
                case Instruction.aload_0:
                    operandStack.Push(locals[0]);
                    return;
                case Instruction.iload_1:
                case Instruction.lload_1:
                case Instruction.fload_1:
                case Instruction.dload_1:
                case Instruction.aload_1:
                    operandStack.Push(locals[1]);
                    return;
                case Instruction.iload_2:
                case Instruction.lload_2:
                case Instruction.fload_2:
                case Instruction.dload_2:
                case Instruction.aload_2:
                    operandStack.Push(locals[2]);
                    return;
                case Instruction.iload_3:
                case Instruction.lload_3:
                case Instruction.fload_3:
                case Instruction.dload_3:
                case Instruction.aload_3:
                    operandStack.Push(locals[3]);
                    return;
                case Instruction.iaload:
                case Instruction.laload:
                case Instruction.faload:
                case Instruction.daload:
                case Instruction.aaload:
                case Instruction.baload:
                case Instruction.caload:
                case Instruction.saload:
                    index = operandStack.PopInt();
                    array = operandStack.PopArray();
                    if (array == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    if (index < 0 || index >= array.Length)
                    {
                        environment.SignalException(ArrayIndexOutOfBoundsExceptionClass.Name, ClassDefaults.IntVoidMethodDescriptor, new object[1] { index });
                        return;
                    }
                    operandStack.Push(array[index]);
                    return;
                    
                case Instruction.istore:
                case Instruction.lstore:
                case Instruction.fstore:
                case Instruction.dstore:
                case Instruction.astore:
                    index = ReadWithWideCheck(code);
                    locals[index] = operandStack.Pop();
                    return;

                case Instruction.istore_0:
                case Instruction.lstore_0:
                case Instruction.fstore_0:
                case Instruction.dstore_0:
                case Instruction.astore_0:
                    locals[0] = operandStack.Pop();
                    return;
                case Instruction.istore_1:
                case Instruction.lstore_1:
                case Instruction.fstore_1:
                case Instruction.dstore_1:
                case Instruction.astore_1:
                    locals[1] = operandStack.Pop();
                    return;
                case Instruction.istore_2:
                case Instruction.lstore_2:
                case Instruction.fstore_2:
                case Instruction.dstore_2:
                case Instruction.astore_2:
                    locals[2] = operandStack.Pop();
                    return;
                case Instruction.istore_3:
                case Instruction.lstore_3:
                case Instruction.fstore_3:
                case Instruction.dstore_3:
                case Instruction.astore_3:
                    locals[3] = operandStack.Pop();
                    return;


                case Instruction.iastore:
                case Instruction.lastore:
                case Instruction.fastore:
                case Instruction.dastore:
                case Instruction.aastore: //TODO: mel by vyhazovat i ArrayStoreException .... JVM 7 spec page 371
                case Instruction.bastore:
                case Instruction.castore:
                case Instruction.sastore:
                    value = operandStack.Pop();
                    index = operandStack.PopInt();
                    array = operandStack.PopArray();
                    if (array == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    if (index < 0 || index >= array.Length)
                    {
                        environment.SignalException(ArrayIndexOutOfBoundsExceptionClass.Name, ClassDefaults.IntVoidMethodDescriptor, new object[1] { index });
                        return;
                    }
                    array[index] = value;
                    return;


                case Instruction.pop:
                    operandStack.Pop();
                    return;
                case Instruction.pop2: // JVM stings.....
                    value = operandStack.Pop();
                    if (!value.IsCategory2Type())
                    {
                        operandStack.Pop();
                    }
                    return;
                case Instruction.dup:
                    Dup();
                    return;
                case Instruction.dup_x1:
                    DupX1();
                    return;
                case Instruction.dup_x2:
                    DupX2();
                    return;
                case Instruction.dup2:
                    Dup2();
                    return;
                case Instruction.dup2_x1:
                    Dup2X1();
                    return;
                case Instruction.dup2_x2:
                    Dup2X2();
                    return;
                case Instruction.swap:
                    value = operandStack.Pop();
                    operandStack.PushMany(value, operandStack.Pop());
                    return;
                case Instruction.iadd:
                    operandStack.Push(operandStack.PopInt() + operandStack.PopInt());
                    return;
                case Instruction.ladd:
                    operandStack.Push(operandStack.PopLong() + operandStack.PopLong());
                    return;
                case Instruction.fadd:
                    operandStack.Push(operandStack.PopFloat() + operandStack.PopFloat());
                    return;
                case Instruction.dadd:
                    operandStack.Push(operandStack.PopDouble() + operandStack.PopDouble());
                    return;
                case Instruction.isub:
                    operandStack.Push(-operandStack.PopInt() + operandStack.PopInt());
                    return;
                case Instruction.lsub:
                    operandStack.Push(-operandStack.PopLong() + operandStack.PopLong());
                    return;
                case Instruction.fsub:
                    operandStack.Push(-operandStack.PopFloat() + operandStack.PopFloat());
                    return;
                case Instruction.dsub:
                    operandStack.Push(-operandStack.PopDouble() + operandStack.PopDouble());
                    return;
                case Instruction.imul:
                    operandStack.Push(operandStack.PopInt() * operandStack.PopInt());
                    return;
                case Instruction.lmul:
                    operandStack.Push(operandStack.PopLong() * operandStack.PopLong());
                    return;
                case Instruction.fmul:
                    operandStack.Push(operandStack.PopFloat() * operandStack.PopFloat());
                    return;
                case Instruction.dmul:
                    operandStack.Push(operandStack.PopDouble() * operandStack.PopDouble());
                    return;

                case Instruction.idiv:
                    iValue = operandStack.PopInt();
                    if (iValue == (int)0)
                    {
                        environment.SignalException(ArithmeticExceptionClass.Name, ClassDefaults.StringVoidMethodDescriptor, new object[1] { environment.CreateStringInstance("/ by zero") });
                        return;
                    }
                    operandStack.Push(operandStack.PopInt() / iValue);
                    return;
                case Instruction.ldiv:
                    lValue = operandStack.PopLong();
                    if (lValue == (long)0)
                    {
                        environment.SignalException(ArithmeticExceptionClass.Name, ClassDefaults.StringVoidMethodDescriptor, new object[1] { environment.CreateStringInstance("/ by zero") });
                        return;
                    }
                    operandStack.Push(operandStack.PopLong() / lValue);
                    return;
                case Instruction.fdiv:
                    fValue = operandStack.PopFloat();
                    operandStack.Push(operandStack.PopFloat() / fValue);
                    return;
                case Instruction.ddiv:
                    dValue = operandStack.PopDouble();
                    operandStack.Push(operandStack.PopDouble() / dValue);
                    return;
                case Instruction.irem:
                    iValue = operandStack.PopInt();
                    if (iValue == (int)0)
                    {
                        environment.SignalException(ArithmeticExceptionClass.Name, ClassDefaults.StringVoidMethodDescriptor, new object[1] { environment.CreateStringInstance("/ by zero") });
                        return;
                    }
                    operandStack.Push(operandStack.PopInt() % iValue);
                    return;
                case Instruction.lrem:
                    lValue = operandStack.PopLong();
                    if (lValue == (long)0)
                    {
                        environment.SignalException(ArithmeticExceptionClass.Name, ClassDefaults.StringVoidMethodDescriptor, new object[1] { environment.CreateStringInstance("/ by zero") });
                        return;
                    }
                    operandStack.Push(operandStack.PopLong() % lValue);
                    return;
                case Instruction.frem:
                    fValue = operandStack.PopFloat();
                    operandStack.Push(operandStack.PopFloat() % fValue);
                    return;
                case Instruction.drem:
                    dValue = operandStack.PopDouble();
                    operandStack.Push(operandStack.PopDouble() % dValue);
                    return;
                case Instruction.ineg:
                    operandStack.Push(-operandStack.PopInt());
                    return;
                case Instruction.lneg:
                    operandStack.Push(-operandStack.PopLong());
                    return;
                case Instruction.fneg:
                    operandStack.Push(-operandStack.PopFloat());
                    return;
                case Instruction.dneg:
                    operandStack.Push(-operandStack.PopDouble());
                    return;
                case Instruction.ishl:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopInt() << iValue);
                    return;
                case Instruction.lshl:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopLong() << iValue);
                    return;
                case Instruction.ishr:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopInt() >> iValue);
                    return;
                case Instruction.lshr:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopLong() >> iValue);
                    return;
                case Instruction.iushr:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopUInt() >> iValue);
                    return;
                case Instruction.lushr:
                    iValue = operandStack.PopInt();
                    operandStack.Push(operandStack.PopULong() >> iValue);
                    return;
                case Instruction.iand:
                    operandStack.Push(operandStack.PopInt() & operandStack.PopInt());
                    return;
                case Instruction.land:
                    operandStack.Push(operandStack.PopLong() & operandStack.PopLong());
                    return;
                case Instruction.ior:
                    operandStack.Push(operandStack.PopInt() | operandStack.PopInt());
                    return;
                case Instruction.lor:
                    operandStack.Push(operandStack.PopLong() | operandStack.PopLong());
                    return;
                case Instruction.ixor:
                    operandStack.Push(operandStack.PopInt() ^ operandStack.PopInt());
                    return;
                case Instruction.lxor:
                    operandStack.Push(operandStack.PopLong() ^ operandStack.PopLong());
                    return;
                case Instruction.iinc:
                    index = ReadWithWideCheck(code, unsetWideFlag: false);
                    iValue = wasWideInstruction ? code.ReadShort(ref pc) : code.ReadSByte(ref pc); //not using ReadWithWideCheck, because here must be SIGNED byte read
                    wasWideInstruction = false;
                    locals[index] = (int)locals[index] + iValue;
                    return;
                case Instruction.i2l:
                    operandStack.Push((long)operandStack.PopInt());
                    return;
                case Instruction.i2f:
                    operandStack.Push((float)operandStack.PopInt());
                    return;
                case Instruction.i2d:
                    operandStack.Push((double)operandStack.PopInt());
                    return;
                case Instruction.l2i:
                    operandStack.Push((int)operandStack.PopLong());
                    return;
                case Instruction.l2f:
                    operandStack.Push((float)operandStack.PopLong());
                    return;
                case Instruction.l2d:
                    operandStack.Push((double)operandStack.PopLong());
                    return;
                case Instruction.f2i:
                    operandStack.Push((int)operandStack.PopFloat());
                    return;
                case Instruction.f2l:
                    operandStack.Push((long)operandStack.PopFloat());
                    return;
                case Instruction.f2d:
                    operandStack.Push((double)operandStack.PopFloat());
                    return;
                case Instruction.d2i:
                    operandStack.Push((int)operandStack.PopDouble());
                    return;
                case Instruction.d2l:
                    operandStack.Push((long)operandStack.PopDouble());
                    return;
                case Instruction.d2f:
                    operandStack.Push((float)operandStack.PopDouble());
                    return;
                case Instruction.i2b:
                    operandStack.Push((int)(byte)operandStack.PopInt());
                    return;
                case Instruction.i2c:
                    operandStack.Push((int)(char)operandStack.PopInt());
                    return;
                case Instruction.i2s:
                    operandStack.Push((int)(short)operandStack.PopInt());
                    return;

                case Instruction.lcmp:
                    LCmp();
                    return;
                case Instruction.fcmpl:
                    FCmp(nanIsOne: false);
                    return;
                case Instruction.fcmpg:
                    FCmp(nanIsOne: true);
                    return;
                case Instruction.dcmpl:
                    DCmp(nanIsOne: false);
                    return;
                case Instruction.dcmpg:
                    DCmp(nanIsOne: true);
                    return;
                case Instruction.ifeq:
                    Jump<int>(v => v == 0, code);
                    return;
                case Instruction.ifne:
                    Jump<int>(v => v != 0, code);
                    return;
                case Instruction.iflt:
                    Jump<int>(v => v < 0, code);
                    return;
                case Instruction.ifge:
                    Jump<int>(v => v >= 0, code);
                    return;
                case Instruction.ifgt:
                    Jump<int>(v => v > 0, code);
                    return;
                case Instruction.ifle:
                    Jump<int>(v => v <= 0, code);
                    return;
                case Instruction.if_icmpeq:
                    JumpTwoValues<int>((v1, v2) => v1 == v2, code);
                    return;
                case Instruction.if_icmpne:
                    JumpTwoValues<int>((v1, v2) => v1 != v2, code);
                    return;
                case Instruction.if_icmplt:
                    JumpTwoValues<int>((v1, v2) => v1 < v2, code);
                    return;
                case Instruction.if_icmpge:
                    JumpTwoValues<int>((v1, v2) => v1 >= v2, code);
                    return;
                case Instruction.if_icmpgt:
                    JumpTwoValues<int>((v1, v2) => v1 > v2, code);
                    return;
                case Instruction.if_icmple:
                    JumpTwoValues<int>((v1, v2) => v1 <= v2, code);
                    return;
                case Instruction.if_acmpeq:
                    JumpTwoValues<JavaInstance>((v1, v2) => v1 == v2, code);
                    return;
                case Instruction.if_acmpne:
                    JumpTwoValues<JavaInstance>((v1, v2) => v1 != v2, code);
                    return;
                case Instruction.ifnull:
                    Jump<JavaInstance>(i => i == null, code);
                    return;
                case Instruction.ifnonnull:
                    Jump<JavaInstance>(i => i != null, code);
                    return;
                case Instruction.@goto:
                    newPC = pc - 1;
                    pc = newPC + (int)code.ReadShort(ref pc);
                    return;
                case Instruction.goto_w:
                    newPC = pc - 1;
                    pc = newPC + code.ReadInt(ref pc);
                    return;
                case Instruction.jsr:
                    newPC = (int)code.ReadShort(ref pc);
                    operandStack.Push(pc);
                    pc = newPC;
                    return;
                case Instruction.jsr_w:
                    newPC = code.ReadInt(ref pc);
                    operandStack.Push(pc);
                    pc = newPC;
                    return;
                case Instruction.ret:
                    index = ReadWithWideCheck(code);
                    pc = (int)locals[index];
                    return;
                case Instruction.tableswitch:
                    TableSwitch(code);
                    return;
                case Instruction.lookupswitch:
                    LookupSwitch(code);
                    return;

                case Instruction.ireturn:
                case Instruction.lreturn:
                case Instruction.freturn:
                case Instruction.dreturn:
                case Instruction.areturn:
                    environment.PrepareReturn(operandStack.Pop());
                    return;
                case Instruction.@return:
                    environment.PrepareReturnVoid();
                    return;

                case Instruction.getstatic:
                    environment.GetStaticFieldValue(GetConstantPoolItem<FieldRefConstantPoolItem>(code));
                    return;
                case Instruction.putstatic:
                    value = operandStack.Pop();
                    environment.SetStaticFieldValue(GetConstantPoolItem<FieldRefConstantPoolItem>(code), value);
                    return;
                case Instruction.getfield:
                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    environment.GetFieldValue(GetConstantPoolItem<FieldRefConstantPoolItem>(code), instance);
                    return;
                case Instruction.putfield:
                    value = operandStack.Pop();
                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    environment.SetFieldValue(GetConstantPoolItem<FieldRefConstantPoolItem>(code), instance, value);
                    return;

                case Instruction.invokestatic:
                    InvokeStatic(code);
                    return;
                case Instruction.invokevirtual:
                    methodRef = GetConstantPoolItem<MethodRefConstantPoolItem>(code);
                    array = ExtractParameters(methodRef);

                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }

                    environment.PrepareVirtualOrInterfaceMethodInvocation(instance, array, methodRef);
                    return;
                case Instruction.invokespecial:
                    methodRef = GetConstantPoolItem<MethodRefConstantPoolItem>(code);
                    array = ExtractParameters(methodRef);

                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }

                    environment.PrepareSpecialMethodInvocation(instance, array, methodRef);
                    return;
                case Instruction.invokeinterface:
                    methodRef = GetConstantPoolItem<MethodRefConstantPoolItem>(code);
                    array = ExtractParameters(methodRef);

                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }

                    environment.PrepareVirtualOrInterfaceMethodInvocation(instance, array, methodRef);
                    pc += 2; //Skip count and 0 parameters 
                    return;

                case Instruction.@new:
                    environment.CreateInstance(GetConstantPoolItem<ClassConstantPoolItem>(code).Name);
                    return;
                case Instruction.newarray:
                    iValue = operandStack.PopInt();
                    if (iValue < 0)
                    {
                        environment.SignalException(NegativeArraySizeExceptionClass.Name);
                        return;
                    }
                    atype = code.ReadByte(ref pc);
                    operandStack.Push(CreateArrayWithDefaultValues(atype, iValue));
                    return;
                case Instruction.anewarray:
                    pc += 2; //skip classRef
                    iValue = operandStack.PopInt();
                    if (iValue < 0)
                    {
                        environment.SignalException(NegativeArraySizeExceptionClass.Name);
                        return;
                    }
                    operandStack.Push(new object[iValue]);
                    return;
                case Instruction.multianewarray:
                    index = code.ReadShort(ref pc);
                    var arrayTypeInfo = Signature.ParseArrayType(ConstantPool.GetItem<ClassConstantPoolItem>(index).Name);
                    int dimensions = code.ReadByte(ref pc);
                    var sizes = new int[dimensions];
                    for (int i = dimensions - 1; i >= 0; i--)
                    {
                        sizes[i] = operandStack.PopInt();
                        if (sizes[i] < 0)
                        {
                            environment.SignalException(NegativeArraySizeExceptionClass.Name);
                            return;
                        }
                    }
                    operandStack.Push(CreateMultiArray(arrayTypeInfo, sizes, 0));
                    return;
                case Instruction.arraylength:
                    array = operandStack.PopArray();
                    if (array == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    operandStack.Push(array.Length);
                    return;
                case Instruction.athrow:
                    instance = operandStack.PopInstance();
                    if (instance == null)
                    {
                        environment.SignalException(NullPointerExceptionClass.Name);
                        return;
                    }
                    environment.SignalException(instance);
                    return;
                
                case Instruction.checkcast:
                    //not implemented but allowed in bytecode (just skiped)
                    pc += 2;
                    return;
                case Instruction.instanceof:
                    throw new NotImplementedException("Instruction instanceof not implemeted!");
                case Instruction.monitorenter:
                case Instruction.monitorexit:
                    throw new NotImplementedException("monitor instructions not implemnted!");
                case Instruction.wide:
                    wasWideInstruction = true;
                    return;
                case Instruction.breakpoint:
                case Instruction.impdep1:
                case Instruction.impdep2:
                    throw new NotImplementedException(string.Format("Reserved instruction {0} is not implemented.", instruction.ToString()));
                case Instruction.invokedynamic:
                    throw new NotImplementedException("invokedynamic instruction is not supported.");
                default:
                    throw new InvalidOperationException("Unknown instruction!");
            }
        }

        

        #region Dup instructions

        private void Dup()
        {
            object value = operandStack.Peek();
            operandStack.Push(value);
        }

        private void DupX1()
        {
            object value1 = operandStack.Pop();
            object value2 = operandStack.Pop();
            operandStack.PushMany(value1, value2, value1);
        }

        private void DupX2()
        {
            object value1 = operandStack.Pop();
            object value2 = operandStack.Pop();

            if (value2.IsCategory2Type())
            {
                operandStack.PushMany(value1, value2, value1);
            }
            else
            {
                object value3 = operandStack.Pop();
                operandStack.PushMany(value1, value3, value2, value1);
            }
        }

        private void Dup2()
        {
            object value1 = operandStack.Pop();
            if (value1.IsCategory2Type())
            {
                operandStack.PushMany(value1, value1);
            }
            else
            {
                object value2 = operandStack.Pop();
                operandStack.PushMany(value2, value1, value2, value1);
            }
        }

        private void Dup2X1()
        {
            object value1 = operandStack.Pop();
            object value2 = operandStack.Pop();
            if (value1.IsCategory2Type())
            {
                operandStack.PushMany(value1, value2, value1);
            }
            else
            {
                object value3 = operandStack.Pop();
                operandStack.PushMany(value2, value1, value3, value2, value1);
            }
        }

        //JVM category 2 types (long, double) hell.... (see JVM specification page 417)
        private void Dup2X2()
        {
            object value1 = operandStack.Pop();
            object value2 = operandStack.Pop();

            if (value1.IsCategory2Type() && value2.IsCategory2Type())
            {// FORM 4
                operandStack.PushMany(value1, value2, value1);
            }
            else
            {
                object value3 = operandStack.Pop();
                if (value3.IsCategory2Type())
                {//FORM 3
                    operandStack.PushMany(value2, value1, value3, value2, value1);
                }
                else
                {
                    if (value1.IsCategory2Type())
                    {//FORM 2
                        operandStack.PushMany(value1, value3, value2, value1);
                    }
                    else
                    {//FORM 1
                        object value4 = operandStack.Pop();
                        operandStack.PushMany(value2, value1, value4, value3, value2, value1);
                    }
                }
            }
        }

        #endregion

        #region CMP instructions

        private void LCmp()
        {
            long value2 = operandStack.PopLong();
            long value1 = operandStack.PopLong();
            if (value1 < value2)
            {
                operandStack.Push((int)-1);
            }
            else if (value1 > value2)
            {
                operandStack.Push((int)1);
            }
            else
            {
                operandStack.Push((int)0);
            }
        }

        private void FCmp(bool nanIsOne)
        {
            float value2 = operandStack.PopFloat();
            float value1 = operandStack.PopFloat();
            if (float.IsNaN(value1) || float.IsNaN(value2))
            {
                operandStack.Push(nanIsOne ? 1 : -1);
            }
            else if (value1 < value2)
            {
                operandStack.Push((int)-1);
            }
            else if (value1 > value2)
            {
                operandStack.Push((int)1);
            }
            else
            {
                operandStack.Push((int)0);
            }
        }

        private void DCmp(bool nanIsOne)
        {
            double value2 = operandStack.PopDouble();
            double value1 = operandStack.PopDouble();
            if (double.IsNaN(value1) || double.IsNaN(value2))
            {
                operandStack.Push(nanIsOne ? (int)1 : (int)-1);
            }
            else if (value1 < value2)
            {
                operandStack.Push((int)-1);
            }
            else if (value1 > value2)
            {
                operandStack.Push((int)1);
            }
            else
            {
                operandStack.Push((int)0);
            }
        }

        #endregion

        #region Jump instructions

        private void Jump<T>(Func<T, bool> when, byte[] code)
        {
            int oldPC = pc - 1;
            int offset = code.ReadShort(ref pc);
            T value = (T)operandStack.Pop();
            if (when(value))
            {
                pc = oldPC + offset;
            }
        }

        private void JumpTwoValues<T>(Func<T, T, bool> when, byte[] code)
        {
            int oldPC = pc - 1;
            int offset = code.ReadShort(ref pc);
            T value2 = (T)operandStack.Pop();
            T value1 = (T)operandStack.Pop();
            if (when(value1, value2))
            {
                pc = oldPC + offset;
            }
        }

        #endregion

        #region Switch instructions

        private void TableSwitch(byte[] code)
        {
            int startPC = pc - 1;

            SkipPadding();

            int defaultCase = code.ReadInt(ref pc);
            int lowValue = code.ReadInt(ref pc);
            int highValue = code.ReadInt(ref pc);

            int value = operandStack.PopInt();

            if (value < lowValue || value > highValue)
            {
                pc = startPC + defaultCase;
            }
            else
            {
                //pc += (value - lowValue) << 2;
                pc += (value - lowValue) * 4;
                pc = startPC + code.ReadInt(ref pc);
            }
        }

        private void LookupSwitch(byte[] code)
        {
            int startPC = pc - 1;

            SkipPadding();

            int defaultCase = code.ReadInt(ref pc);
            int nPairsLength = code.ReadInt(ref pc);

            int value = operandStack.PopInt();
            int matchOffsetPairsStart = pc;

            bool foundCase = false;

            //binary search for matching switch case
            int min = 1;
            int max = nPairsLength;
            int middle;
            int currentIndex;
            int currentValue;

            while (max >= min)
            {
                middle = (max - min) / 2 + min;
                currentIndex = middle * 8 + matchOffsetPairsStart;
                currentValue = code.ReadInt(ref currentIndex);
                if (currentValue < value)
                {
                    min = middle + 1;
                }
                else if (currentValue > value)
                {
                    max = middle - 1;
                }
                else
                {
                    pc = startPC + code.ReadInt(ref currentIndex);
                    foundCase = true;
                }
            }

            if (!foundCase)
            {
                pc = startPC + defaultCase;
            }
        }

        private void SkipPadding()
        {
            while ((pc % 4) != 0)
            {
                pc++;
            }
        }

        #endregion

        #region Array instructions

        private object[] CreateArrayWithDefaultValues(byte atype, int length)
        {
            switch (atype)
            {
                case 4: //boolean
                case 5: //char
                case 8: //byte
                case 9: //short
                case 10: //int
                    return Enumerable.Repeat<object>((int)0, length).ToArray();
                case 6: //float
                    return Enumerable.Repeat<object>((float)0.0, length).ToArray();
                case 7: //double
                    return Enumerable.Repeat<object>((double)0.0, length).ToArray();
                case 11: //long
                    return Enumerable.Repeat<object>((long)0, length).ToArray();
                default:
                    throw new InvalidOperationException("Unknown atype in newarray instruction!");
            }
        }

        private object CreateMultiArray(ArrayTypeInfo arrayTypeInfo, int[] sizes, int index)
        {
            if (index == sizes.Length)
            {
                switch (arrayTypeInfo.ElementType.Type)
                {
                    case JVMType.Boolean:
                    case JVMType.Char:
                    case JVMType.Byte:
                    case JVMType.Short:
                    case JVMType.Int:
                        return (int)0;
                    case JVMType.Float:
                        return (float)0.0;
                    case JVMType.Double:
                        return (double)0.0;
                    case JVMType.Long:
                        return (long)0;
                    case JVMType.Object:
                        return null;
                    default:
                        throw new InvalidOperationException("Illegal element type in multianewarray instruction!");
                }
            }

            int size = sizes[index];
            object[] array = new object[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = CreateMultiArray(arrayTypeInfo, sizes, index + 1);
            }
            return array;
        }

        #endregion

        #region Invoke instructions

        private void InvokeStatic(byte[] code)
        {
            var methodRef = GetConstantPoolItem<MethodRefConstantPoolItem>(code);
            var parameters = ExtractParameters(methodRef);

            environment.PrepareStaticMethodInvocation(parameters, methodRef);
        }

        private object[] ExtractParameters(MethodRefConstantPoolItem methodRef)
        {
            int parametersCount = methodRef.Signature.ParametersCount;
            object[] parameters = new object[parametersCount];

            for (int i = parametersCount - 1; i >= 0; i--)
            {
                parameters[i] = operandStack.Pop();
            }

            return parameters;
        }

        #endregion

        #region Helper methods

        private int ReadWithWideCheck(byte[] code, bool unsetWideFlag = true)
        {
            int returnValue = wasWideInstruction ? code.ReadShort(ref pc) : code.ReadByte(ref pc);

            if (unsetWideFlag)
            {
                wasWideInstruction = false;
            }
            return returnValue;
        }

        private T GetConstantPoolItem<T>(byte[] code) where T : ConstantPoolItemBase
        {
            int index = code.ReadShort(ref pc);
            return classObject.ConstantPool.GetItem<T>(index);
        }

        #endregion
    }


    internal static class CodeExtensions
    {
        public static byte ReadByte(this byte[] bytes, ref int pc)
        {
            return bytes[pc++];
        }

        public static sbyte ReadSByte(this byte[] bytes, ref int pc)
        {
            return (sbyte)bytes[pc++];
        }

        public static short ReadShort(this byte[] bytes, ref int pc)
        {
            short s = (short)((bytes[pc] << 8) | bytes[pc + 1]);
            pc += 2;
            return s;
        }

        public static int ReadInt(this byte[] bytes, ref int pc)
        {
            int i = (int)((bytes[pc] << 24) + (bytes[pc + 1] << 16) + (bytes[pc + 2] << 8) + bytes[pc + 3]);
            pc += 4;
            return i;
        }



        public static bool IsCategory2Type(this object obj)
        {
            return (obj is long || obj is double);
        }
    }

    internal static class StackExtensions
    {
        public static void PushMany(this Stack<object> stack, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                stack.Push(values[i]);
            }
        }

        public static int PopInt(this Stack<object> stack)
        {
            return (int)stack.Pop();
        }

        public static uint PopUInt(this Stack<object> stack)
        {
            return (uint)stack.Pop();
        }

        public static long PopLong(this Stack<object> stack)
        {
            return (long)stack.Pop();
        }

        public static ulong PopULong(this Stack<object> stack)
        {
            return (ulong)stack.Pop();
        }

        public static float PopFloat(this Stack<object> stack)
        {
            return (float)stack.Pop();
        }

        public static double PopDouble(this Stack<object> stack)
        {
            return (double)stack.Pop();
        }

        public static JavaInstance PopInstance(this Stack<object> stack)
        {
            return (JavaInstance)stack.Pop();
        }

        public static object[] PopArray(this Stack<object> stack)
        {
            return (object[])stack.Pop();
        }
    }
}
