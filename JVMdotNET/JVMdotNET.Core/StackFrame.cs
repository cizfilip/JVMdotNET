using JVMdotNET.Core.Bytecode;
using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal class StackFrame
    {
        private int pc;        
        private bool wasWideInstruction;

        private object[] locals;
        private Stack<object> operandStack;

        private JavaClass classObject;
        private MethodInfo method;

        public StackFrame(JavaClass @class, MethodInfo method)
        {
            this.pc = 0;
            this.wasWideInstruction = true;
            this.classObject = @class;
            this.method = method;

            var code = method.Code;


            this.locals = new object[code.MaxLocals];
            this.operandStack = new Stack<object>(code.MaxStack);
        }

        public void Unload()
        {
            this.classObject = null;
            this.method = null;
        }

        public void Run(RuntimeEnvironment env)
        {
            CodeAttribute codeInfo = method.Code;
            byte[] code = codeInfo.Code;
            int codeLength = code.Length;
            int index = 0;

            while (pc < codeLength)
            {
                var instruction = (Instruction)code.ReadByte(ref pc);

                switch (instruction)
                {
                    case Instruction.nop:
                        break;
                    case Instruction.aconst_null:
                        operandStack.Push(NullInstance.Value);
                        break;
                    case Instruction.iconst_m1:
                        operandStack.Push((int)-1);
                        break;
                    case Instruction.iconst_0:
                        operandStack.Push((int)0);
                        break;
                    case Instruction.iconst_1:
                        operandStack.Push((int)1);
                        break;
                    case Instruction.iconst_2:
                        operandStack.Push((int)2);
                        break;
                    case Instruction.iconst_3:
                        operandStack.Push((int)3);
                        break;
                    case Instruction.iconst_4:
                        operandStack.Push((int)4);
                        break;
                    case Instruction.iconst_5:
                        operandStack.Push((int)5);
                        break;
                    case Instruction.lconst_0:
                        operandStack.Push((long)0);
                        break;
                    case Instruction.lconst_1:
                        operandStack.Push((long)1);
                        break;
                    case Instruction.fconst_0:
                        operandStack.Push((float)0.0);
                        break;
                    case Instruction.fconst_1:
                        operandStack.Push((float)1.0);
                        break;
                    case Instruction.fconst_2:
                        operandStack.Push((float)2.0);
                        break;
                    case Instruction.dconst_0:
                        operandStack.Push((double)0.0);
                        break;
                    case Instruction.dconst_1:
                        operandStack.Push((double)1.0);
                        break;
                    case Instruction.bipush:
                        operandStack.Push((int)code.ReadSByte(ref pc));
                        break;
                    case Instruction.sipush:
                        operandStack.Push((int)code.ReadShort(ref pc));
                        break;
                    case Instruction.ldc:
                        throw new NotImplementedException();
                        break;
                    case Instruction.ldc_w:
                        throw new NotImplementedException();
                        break;
                    case Instruction.ldc2_w:
                        throw new NotImplementedException();
                        break;
                    case Instruction.iload:
                    case Instruction.lload:
                    case Instruction.fload:
                    case Instruction.dload:
                    case Instruction.aload:
                        index = wasWideInstruction ? code.ReadShort(ref pc) : code.ReadByte(ref pc);
                        operandStack.Push(locals[index]);
                        wasWideInstruction = false;
                        break;
                    case Instruction.iload_0:
                    case Instruction.lload_0:
                    case Instruction.fload_0:
                    case Instruction.dload_0:
                    case Instruction.aload_0:
                        operandStack.Push(locals[0]);
                        break;
                    case Instruction.iload_1:
                    case Instruction.lload_1:
                    case Instruction.fload_1:
                    case Instruction.dload_1:
                    case Instruction.aload_1:
                        operandStack.Push(locals[1]);
                        break;
                    case Instruction.iload_2:
                    case Instruction.lload_2:
                    case Instruction.fload_2:
                    case Instruction.dload_2:
                    case Instruction.aload_2:
                        operandStack.Push(locals[2]);
                        break;
                    case Instruction.iload_3:
                    case Instruction.lload_3:
                    case Instruction.fload_3:
                    case Instruction.dload_3:
                    case Instruction.aload_3:
                        operandStack.Push(locals[3]);
                        break;
                    case Instruction.iaload: //array loads must throws NullReferenceExp or IndexOutOfRangeExp... how????
                        break;
                    case Instruction.laload:
                        break;
                    case Instruction.faload:
                        break;
                    case Instruction.daload:
                        break;
                    case Instruction.aaload:
                        break;
                    case Instruction.baload:
                        break;
                    case Instruction.caload:
                        break;
                    case Instruction.saload:
                        break;


                    case Instruction.istore: //wide check
                    case Instruction.lstore:
                    case Instruction.fstore:
                    case Instruction.dstore:
                    case Instruction.astore:
                        index = wasWideInstruction ? code.ReadShort(ref pc) : code.ReadByte(ref pc);
                        locals[index] = operandStack.Pop();
                        wasWideInstruction = false;
                        break;

                    case Instruction.istore_0:
                    case Instruction.lstore_0:
                    case Instruction.fstore_0:
                    case Instruction.dstore_0:
                    case Instruction.astore_0:
                        locals[0] = operandStack.Pop();
                        break;
                    case Instruction.istore_1:
                    case Instruction.lstore_1:
                    case Instruction.fstore_1:
                    case Instruction.dstore_1:
                    case Instruction.astore_1:
                        locals[1] = operandStack.Pop();
                        break;
                    case Instruction.istore_2:
                    case Instruction.lstore_2:
                    case Instruction.fstore_2:
                    case Instruction.dstore_2:
                    case Instruction.astore_2:
                        locals[2] = operandStack.Pop();
                        break;
                    case Instruction.istore_3:
                    case Instruction.lstore_3:
                    case Instruction.fstore_3:
                    case Instruction.dstore_3:
                    case Instruction.astore_3:
                        locals[3] = operandStack.Pop();
                        break;
                    

                    case Instruction.iastore:
                        break;
                    case Instruction.lastore:
                        break;
                    case Instruction.fastore:
                        break;
                    case Instruction.dastore:
                        break;
                    case Instruction.aastore:
                        break;
                    case Instruction.bastore:
                        break;
                    case Instruction.castore:
                        break;
                    case Instruction.sastore:
                        break;
                    case Instruction.pop:
                        break;
                    case Instruction.pop2:
                        break;
                    case Instruction.dup:
                        break;
                    case Instruction.dup_x1:
                        break;
                    case Instruction.dup_x2:
                        break;
                    case Instruction.dup2:
                        break;
                    case Instruction.dup2_x1:
                        break;
                    case Instruction.dup2_x2:
                        break;
                    case Instruction.swap:
                        break;
                    case Instruction.iadd:
                        break;
                    case Instruction.ladd:
                        break;
                    case Instruction.fadd:
                        break;
                    case Instruction.dadd:
                        break;
                    case Instruction.isub:
                        break;
                    case Instruction.lsub:
                        break;
                    case Instruction.fsub:
                        break;
                    case Instruction.dsub:
                        break;
                    case Instruction.imul:
                        break;
                    case Instruction.lmul:
                        break;
                    case Instruction.fmul:
                        break;
                    case Instruction.dmul:
                        break;
                    case Instruction.idiv:
                        break;
                    case Instruction.ldiv:
                        break;
                    case Instruction.fdiv:
                        break;
                    case Instruction.ddiv:
                        break;
                    case Instruction.irem:
                        break;
                    case Instruction.lrem:
                        break;
                    case Instruction.frem:
                        break;
                    case Instruction.drem:
                        break;
                    case Instruction.ineg:
                        break;
                    case Instruction.lneg:
                        break;
                    case Instruction.fneg:
                        break;
                    case Instruction.dneg:
                        break;
                    case Instruction.ishl:
                        break;
                    case Instruction.lshl:
                        break;
                    case Instruction.ishr:
                        break;
                    case Instruction.lshr:
                        break;
                    case Instruction.iushr:
                        break;
                    case Instruction.lushr:
                        break;
                    case Instruction.iand:
                        break;
                    case Instruction.land:
                        break;
                    case Instruction.ior:
                        break;
                    case Instruction.lor:
                        break;
                    case Instruction.ixor:
                        break;
                    case Instruction.lxor:
                        break;
                    case Instruction.iinc: //wide check
                        break;
                    case Instruction.i2l:
                        break;
                    case Instruction.i2f:
                        break;
                    case Instruction.i2d:
                        break;
                    case Instruction.l2i:
                        break;
                    case Instruction.l2f:
                        break;
                    case Instruction.l2d:
                        break;
                    case Instruction.f2i:
                        break;
                    case Instruction.f2l:
                        break;
                    case Instruction.f2d:
                        break;
                    case Instruction.d2i:
                        break;
                    case Instruction.d2l:
                        break;
                    case Instruction.d2f:
                        break;
                    case Instruction.i2b:
                        break;
                    case Instruction.i2c:
                        break;
                    case Instruction.i2s:
                        break;
                    case Instruction.lcmp:
                        break;
                    case Instruction.fcmpl:
                        break;
                    case Instruction.fcmpg:
                        break;
                    case Instruction.dcmpl:
                        break;
                    case Instruction.dcmpg:
                        break;
                    case Instruction.ifeq:
                        break;
                    case Instruction.ifne:
                        break;
                    case Instruction.iflt:
                        break;
                    case Instruction.ifge:
                        break;
                    case Instruction.ifgt:
                        break;
                    case Instruction.ifle:
                        break;
                    case Instruction.if_icmpeq:
                        break;
                    case Instruction.if_icmpne:
                        break;
                    case Instruction.if_icmplt:
                        break;
                    case Instruction.if_icmpge:
                        break;
                    case Instruction.if_icmpgt:
                        break;
                    case Instruction.if_icmple:
                        break;
                    case Instruction.if_acmpeq:
                        break;
                    case Instruction.if_acmpne:
                        break;
                    case Instruction.@goto:
                        break;
                    case Instruction.jsr:
                        break;
                    case Instruction.ret: //wide check
                        break;
                    case Instruction.tableswitch:
                        break;
                    case Instruction.lookupswitch:
                        break;
                    case Instruction.ireturn:
                        break;
                    case Instruction.lreturn:
                        break;
                    case Instruction.freturn:
                        break;
                    case Instruction.dreturn:
                        break;
                    case Instruction.areturn:
                        break;
                    case Instruction.@return:
                        break;
                    case Instruction.getstatic:
                        break;
                    case Instruction.putstatic:
                        break;
                    case Instruction.getfield:
                        break;
                    case Instruction.putfield:
                        break;
                    case Instruction.invokevirtual:
                        break;
                    case Instruction.invokespecial:
                        break;
                    case Instruction.invokestatic:
                        break;
                    case Instruction.invokeinterface:
                        break;
                    case Instruction.invokedynamic:
                        break;
                    case Instruction.@new:
                        break;
                    case Instruction.newarray:
                        break;
                    case Instruction.anewarray:
                        break;
                    case Instruction.arraylength:
                        break;
                    case Instruction.athrow:
                        break;
                    case Instruction.checkcast:
                        break;
                    case Instruction.instanceof:
                        break;
                    case Instruction.monitorenter:
                        break;
                    case Instruction.monitorexit:
                        break;
                    case Instruction.wide:
                        wasWideInstruction = true;
                        break;
                    case Instruction.multianewarray:
                        break;
                    case Instruction.ifnull:
                        break;
                    case Instruction.ifnonnull:
                        break;
                    case Instruction.goto_w:
                        break;
                    case Instruction.jsr_w:
                        break;
                    case Instruction.breakpoint:
                        break;
                    case Instruction.impdep1:
                        break;
                    case Instruction.impdep2:
                        break;
                    default:
                        break;
                }
            }
        }


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
    }
}
