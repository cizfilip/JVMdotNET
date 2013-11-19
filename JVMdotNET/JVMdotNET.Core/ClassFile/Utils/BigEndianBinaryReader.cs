using JVMdotNET.Core.Exceptions;
using System;
using System.IO;

namespace JVMdotNET.Core.ClassFile.Utils
{
    internal sealed class BigEndianBinaryReader : BinaryReader
    {
        internal BigEndianBinaryReader(Stream input)
            : base(input)
        {

        }

        public override uint ReadUInt32()
        {
            uint i = base.ReadUInt32();
            return ((i & 0xFF) << 24) | (i >> 24) |
                   ((i << 8) & 0x00FF0000) |
                   ((i >> 8) & 0x0000FF00);
        }

        public override ushort ReadUInt16()
        {
            ushort i = base.ReadUInt16();

            return (ushort)((i >> 8) | (i << 8));
        }

        public override short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public override int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public override double ReadDouble()
        {
            byte[] b = new byte[8];
            Read(b, 0, 8);
            byte bb = b[0];
            b[0] = b[7];
            b[7] = bb;
            bb = b[1];
            b[1] = b[6];
            b[6] = bb;
            bb = b[2];
            b[2] = b[5];
            b[5] = bb;
            bb = b[3];
            b[3] = b[4];
            b[4] = bb;

            BinaryReader br = new BinaryReader(new MemoryStream(b));
            return br.ReadDouble();
        }

        public override long ReadInt64()
        {
            byte[] b = new byte[8];
            Read(b, 0, 8);
            byte bb = b[0];
            b[0] = b[7];
            b[7] = bb;
            bb = b[1];
            b[1] = b[6];
            b[6] = bb;
            bb = b[2];
            b[2] = b[5];
            b[5] = bb;
            bb = b[3];
            b[3] = b[4];
            b[4] = bb;

            BinaryReader br = new BinaryReader(new MemoryStream(b));
            return br.ReadInt64();
        }

        public override float ReadSingle()
        {
            byte[] b = new byte[4];
            Read(b, 0, 4);
            byte bb = b[0];
            b[0] = b[3];
            b[3] = bb;
            bb = b[1];
            b[1] = b[2];
            b[2] = bb;

            BinaryReader br = new BinaryReader(new MemoryStream(b));
            return br.ReadSingle();
        }

        public override string ReadString()
        {
            int len = ReadUInt16();
            byte[] buf = new byte[len];
            int pos = 0;
            
            // special code path for ASCII strings (which occur *very* frequently)
            for (int j = 0; j < len; j++)
            {
                if (buf[pos + j] == 0 || buf[pos + j] >= 128)
                {
                    // NOTE we *cannot* use System.Text.UTF8Encoding, because this is *not* compatible
                    // (esp. for embedded nulls)
                    char[] ch = new char[len];
                    int l = 0;
                    for (int i = 0; i < len; i++)
                    {
                        int c = buf[pos + i];
                        int char2, char3;
                        switch (c >> 4)
                        {
                            case 0:
                                if (c == 0)
                                {
                                    ThrowIllegalUtf8String();
                                }
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                // 0xxxxxxx
                                break;
                            case 12:
                            case 13:
                                // 110x xxxx   10xx xxxx
                                char2 = buf[pos + ++i];
                                if ((char2 & 0xc0) != 0x80 || i >= len)
                                {
                                    ThrowIllegalUtf8String();
                                }
                                c = (((c & 0x1F) << 6) | (char2 & 0x3F));
                                break;
                            case 14:
                                // 1110 xxxx  10xx xxxx  10xx xxxx
                                char2 = buf[pos + ++i];
                                char3 = buf[pos + ++i];
                                if ((char2 & 0xc0) != 0x80 || (char3 & 0xc0) != 0x80 || i >= len)
                                {
                                    ThrowIllegalUtf8String();
                                }
                                c = (((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0));
                                break;
                            default:
                                ThrowIllegalUtf8String();
                                break;
                        }
                        ch[l++] = (char)c;
                    }
                    return new String(ch, 0, l);
                }
            }
            string s = System.Text.ASCIIEncoding.ASCII.GetString(buf, pos, len);
            return s;
        }

        private void ThrowIllegalUtf8String()
        {
            throw new ClassFileFormatException("Illegal UTF8 string in constant pool.");
        }
    }

}
